using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiDC2.Tridy
{
    public class Auto
    {
        private int ID { get; set; }
        public string SPZ { get; set; }
        public string Barva { get; set; }
        public string Model { get; set; }

        // Reference na Auto
        public Ridic Ridic { get; set; }
    }

}
