using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiDC2.Tridy
{
    public class Jizda
    {
        private int ID { get; set; }
        public string Jmeno { get; set; }
        public string Cislo { get; set; }
        public string Odkud { get; set; }
        public string Kam { get; set; }
        public DateTime Kdy { get; set; }

        // Reference na Řidiče
        public Ridic Ridic { get; set; }
        public string Status { get; set; }
        public string Poznamka { get; set; }
    }

}
