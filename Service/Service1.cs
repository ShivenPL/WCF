using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    [DataContract]
    public class LotNieZnalezionoExeption
    {
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class NieZnalezionoMIastaExeprion
    {
        [DataMember]
        public string Message { get; set; }
    }
    // UWAGA: możesz użyć polecenia „Zmień nazwę” w menu „Refaktoryzuj”, aby zmienić nazwę klasy „Service1” w kodzie i pliku konfiguracji.
    public class Service1 : IService1
    {

        private const string ścieżka = @"C:\Users\kryst\source\repos\Service\loty.csv";
        public List<Lot> loty = null;

        public List<Lot> Loty
        {
            get
            {
                if (loty == null)
                {
                    Inicjalizuj();
                }
                return loty;
            }
        }

        public Service1()
        {
             Inicjalizuj();
        }

        private string pobierzZPliku()
        {
            string zawartośćCSV = null;

            using (TextReader reader = new StreamReader(ścieżka))
            {
                zawartośćCSV = reader.ReadToEnd();
            }

            return zawartośćCSV;
        }


        public void Inicjalizuj()
        {
            loty = new List<Lot>();
            string[] linie = pobierzZPliku().Split("\n".ToCharArray());
            foreach (string linia in linie)
            {
                try
                {
                    string[] parametryLotu = linia.Split(",".ToCharArray());
                    int ID = Int32.Parse(parametryLotu[0]);
                    DateTime wylot = DateTime.Parse(parametryLotu[3]);
                    DateTime przylot = DateTime.Parse(parametryLotu[4]);
                    loty.Add(new Lot(ID, new Lotnisko(parametryLotu[1]), new Lotnisko(parametryLotu[2]), wylot, przylot));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd w linii: " + linia + "\nWyjątek: " + ex.Message);
                }
            }
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public List<Lot> GetLots(string portA, string portB, DateTime przedzialOd, DateTime przedzialDo)
        {
            List<Lot> list = new List<Lot>();
            Boolean portaEx = false;
            Boolean portbEx = false;
            foreach (Lot lot in loty)
            {
                if (lot.skad.miasto == portA)
                    portaEx = true;
                if (lot.dokad.miasto == portB)
                    portbEx = true;
            }

            if(portaEx == false)
            {
                NieZnalezionoMIastaExeprion nieZnalezionoA = new NieZnalezionoMIastaExeprion { Message = "Nie znaleziono takiego portu źródłowego: " + portA };
                throw new FaultException<NieZnalezionoMIastaExeprion>(nieZnalezionoA, new FaultReason("Nie znaleziono takiego portu źródłowego: " + portA));
            }

            if (portbEx == false)
            {
                NieZnalezionoMIastaExeprion nieZnalezionoB = new NieZnalezionoMIastaExeprion { Message = "Nie znaleziono takiego portu docelowego: " + portB };
                throw new FaultException<NieZnalezionoMIastaExeprion>(nieZnalezionoB, new FaultReason("Nie znaleziono takiego portu docelowego: " + portB));
            }

            if (przedzialDo == DateTime.Parse("01.01.0001 00:00:00") || przedzialOd == DateTime.Parse("01.01.0001 00:00:00"))
            {
                foreach (Lot lot in loty)
                {
                    if (lot.skad.miasto == portA && lot.dokad.miasto == portB)
                    {
                        list.Add(lot);
                    }
                }
            }
            else
            {
                foreach (Lot lot in loty)
                {
                    if (lot.skad.miasto == portA && lot.dokad.miasto == portB && lot.godzinaOdlotu >= przedzialOd && lot.godzinaPrzylotu <= przedzialDo )
                    {
                        list.Add(lot);
                    }
                }
            }

            if (list.Count == 0)
            {
                LotNieZnalezionoExeption NieZnalezionoExeption = new LotNieZnalezionoExeption { Message = "Nie znaleziono takiego połączenia" };
                throw new FaultException<LotNieZnalezionoExeption>(NieZnalezionoExeption, new FaultReason("Nie znaleziono takiego połączenia"));
            }
                
            

            return list;
        }

        
    }

    

}
