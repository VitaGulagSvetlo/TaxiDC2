using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxiDC2.Models
{
    public class Driver : BaseModel
    {
        private bool _active;
        private bool _admin;
        private bool _notificationEnable;

		public Guid IdDriver { get; set; }
        [StringLength(50)]
        [Required]
        public string? FirstName { get; set; }
        [StringLength(50)]
        [Required]
        public string? LastName { get; set; }
        [StringLength(20)]
        [Required]
        public string? PhoneNumber { get; set; }

        [StringLength(500)]
        public string? MobileDeviceKey { get; set; }

        [StringLength(500)]
        public string? MobileDeviceHash { get; set; }

        public bool Active
        {
            get => _active;
            set
            {
                IsDirty = _active != value;
                _active = value;
            }
        }

        public bool NotificationEnable
        {
            get => _notificationEnable;
            set
            {
                IsDirty = _notificationEnable != value;
                _notificationEnable = value;
            }
        }

        public Guid? AssignedCar { get; set; }

        public Car? Car { get; set; }

        public string FullName => $"{LastName} {FirstName}";

        public string Inicials => $"{FirstName?.Substring(0, 1)}{LastName?.Substring(0, 1)}".ToUpper();

        public bool IsAdmin
        {
            get => _admin;
            set
            {
                IsDirty = _admin != value;
                _admin = value;
            }
        }

        public string Email { get; set; }


        
        public Color LightColor => Active ? Colors.Green : Colors.SlateGrey;

		public Color ShieldColor => IsAdmin ? Colors.Green : Colors.SlateGrey;

		public Color BellColor => NotificationEnable?Colors.Green:Colors.SlateGrey;

	}

}
