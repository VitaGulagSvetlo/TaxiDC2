using System.ComponentModel.DataAnnotations;

namespace TaxiDC2.Models
{
    public class Driver : BaseModel
    {
		public Guid IdDriver { get; set; }
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }
        [StringLength(20)]
        [Required]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        public string MobileDeviceKey { get; set; }

        [StringLength(500)]
        public string MobileDeviceHash { get; set; }

        public bool Active { get; set; }

		public bool NotificationEnable { get; set; }

		public Car Car { get; set; }

        public bool IsAdmin { get; set; }

        public string Email { get; set; }

		public string FullName => $"{LastName} {FirstName}";

		public string Inicials => $"{FirstName?.Substring(0, 1)}{LastName?.Substring(0, 1)}".ToUpper();

    }

}
