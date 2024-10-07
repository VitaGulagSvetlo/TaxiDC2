using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiDC2.Tridy
{
    public class Ridic
    {
        private int ID { get; set; }
        public string Jmeno { get; set; }
        public string Mobil { get; set; }
        public string Cislo { get; set; }
        public bool JeActive { get; set; }
        public bool OznameniActive { get; set; }
        public bool JeAdmin { get; set; }

        // Reference na Auto
        public Auto Auto { get; set; }
    }

}
