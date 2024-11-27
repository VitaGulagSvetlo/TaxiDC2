using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiDC2.Models
{
    public class Log : BaseModel
    {
        [Key]
        public long IdLog { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Text { get; set; } = string.Empty;

        public string User { get; set; } = string.Empty;

        public Guid? TripId { get; set; }

        [field: NonSerialized]
        public Trip? Trip { get; set; }
    }

    public class Lokace
    {

        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

    }
}
