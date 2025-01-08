using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDC2.Models.Enums;

namespace TaxiDC2.Models
{
    public class Trip : BaseModel
    {
        public Guid IdTrip { get; set; } = Guid.Empty;

        [StringLength(500)]
        [Required]
        public string AddressBoarding { get; set; } = string.Empty;

        [StringLength(500)]
        [Required]
        public string AddressExit { get; set; } = string.Empty;

        [Required]
        public DateTime OrderTime { get; set; } = DateTime.Now;

        public DateTime? BoardingTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public bool Complete { get; set; } = false;
        public Customer? Customer { get; set; } = null;
        public Driver? Driver { get; set; } = null;
        public TimeSpan? DeadLine { get; set; }
        public TripState TripState { get; set; } = TripState.NewOrder;

        public bool AddressBoardingIsValid { get; set; }
        public bool AddressExitIsValid { get; set; }

        [StringLength(20)]
        public string AddressBoardingLocX { get; set; } = String.Empty;

        [StringLength(20)]
        public string AddressBoardingLocY { get; set; } = String.Empty;

        [StringLength(20)]
        public string AddressExitLocX { get; set; } = String.Empty;

        [StringLength(20)]
        public string AddressExitLocY { get; set; } = String.Empty;

        public List<Log> Logs { get; set; } = new List<Log>();
    }

}
