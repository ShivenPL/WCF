using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [DataContract]
    public class Lot
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public Lotnisko skad { get; set; }
        [DataMember]
        public Lotnisko dokad { get; set; }
        [DataMember]
        public DateTime godzinaOdlotu { get; set; }
        [DataMember]
        public DateTime godzinaPrzylotu { get; set; }

        public Lot(int _ID, Lotnisko _skad, Lotnisko _dokod, DateTime _godzinaOdlotu, DateTime _godzinaPrzylotu)
        {
            ID = _ID;
            skad = _skad;
            dokad = _dokod;
            godzinaOdlotu = _godzinaOdlotu;
            godzinaPrzylotu = _godzinaPrzylotu;
        }

        public static implicit operator List<object>(Lot v)
        {
            throw new NotImplementedException();
        }
    }
}
