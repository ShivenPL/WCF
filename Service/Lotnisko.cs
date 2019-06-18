using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [DataContract]
    public class Lotnisko
    {
        [DataMember]
        public string miasto { get; set; }

        public Lotnisko(string _miasto)
        {
            miasto = _miasto;
        }
    }
}
