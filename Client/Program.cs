using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace Client
{
    class Program
    {
        static IService1 service1;
        static List<Lot> loty = new List<Lot>();
        static void Main(string[] args)
        {
            var myBinding = new BasicHttpBinding();
            var myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/Service/Service1/mex");
            ChannelFactory<IService1> channelFactory = new ChannelFactory<IService1>(myBinding, myEndpoint);
            try
            {
                service1 = channelFactory.CreateChannel();
            
                Boolean exitFlag = false;
                String portA, portB, czasOd, czasDo;
                do
                {
                    Console.WriteLine("Menu!");
                    Console.WriteLine("1: Sprawdź loty z portA do portB");
                    Console.WriteLine("2: Sprawdź loty z portA do portB z przedziałem czasowym");
                    Console.WriteLine("0: Wyjdz");
                    string switcher = Console.ReadLine();
                    switch (switcher)
                    {
                        case "1":
                            Console.WriteLine("Wybrales 1.");
                            Console.WriteLine("Podaj portA: ");
                            portA = Console.ReadLine();
                            Console.WriteLine("Podaj portB: ");
                            portB = Console.ReadLine();
                            Console.WriteLine("Wysylanie zapytania...");
                            try
                            {
                                try
                                {
                                    loty = service1.GetLots(portA, portB, DateTime.Parse("01.01.0001 00:00:00"), DateTime.Parse("01.01.0001 00:00:00"));
                                }
                                catch (FaultException e)
                                {
                                    Console.WriteLine(e.Message);
                                    break;
                                }
                            foreach (Lot lot in loty)
                                {
                                    Console.WriteLine("Lot z: " + lot.skad.miasto + " do: " + lot.dokad.miasto + ": Odlot " + lot.godzinaOdlotu + ", szacowana godzina przylotu: " + lot.godzinaPrzylotu);
                                }
                                break;
                            }
                            
                            catch (FaultException<LotNieZnalezionoExeption> e)
                            {
                                Console.WriteLine(e.Detail.Message);
                                break;
                            }

                        case "2":
                            Console.WriteLine("Wybrales 1.");
                            Console.WriteLine("Podaj portA: ");
                            portA = Console.ReadLine();
                            Console.WriteLine("Podaj portB: ");
                            portB = Console.ReadLine();
                            Console.WriteLine("Podaj przedział od: ");
                            czasOd = Console.ReadLine();
                            Console.WriteLine("Podaj przedział do: ");
                            czasDo = Console.ReadLine();
                            Console.WriteLine("Wysylanie zapytania...");
                            try
                            {
                                loty = service1.GetLots(portA, portB, DateTime.Parse(czasOd), DateTime.Parse(czasDo));

                                foreach (Lot lot in loty)
                                {
                                    Console.WriteLine("Lot z: " + lot.skad.miasto + " do: " + lot.dokad.miasto + ": Odlot " + lot.godzinaOdlotu + ", szacowana godzina przylotu: " + lot.godzinaPrzylotu);
                                }
                                break;
                            }
                            catch (FaultException<NieZnalezionoMIastaExeprion> e)
                            {
                                Console.WriteLine(e.Detail.Message);
                                break;
                            }
                            catch (FaultException<LotNieZnalezionoExeption> e)
                            {
                                Console.WriteLine(e.Detail.Message);
                                break;
                            }


                        case "0":
                            Console.WriteLine("Nastapi wyjscie");
                            ((ICommunicationObject)service1).Close();
                            channelFactory.Close();
                            exitFlag = true;
                            break;

                        default:
                            Console.WriteLine("Zły wybór!");
                            break;
                    }
                } while (!exitFlag);
              
            Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd połączenia z serwerem " + e);
            }
        }
    }
}
