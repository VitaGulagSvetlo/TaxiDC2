using System.ComponentModel.DataAnnotations;

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
}
