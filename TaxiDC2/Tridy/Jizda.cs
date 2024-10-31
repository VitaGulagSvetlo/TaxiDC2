using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiDC2.Tridy
{
    public class Jizda
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }        
        public Color ItemBackgroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public Color IconColor { get; set; }
        public string Icon { get; set; }

        public double Rating { get; set; }
        
        // Reference na Řidiče
        public Ridic Ridic { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }

    }

}
