using System.ComponentModel.DataAnnotations;

namespace TaxiDC2.Models
{
    public class Customer : BaseModel
    {
        private bool _vip;
        public Guid IdCustomer { get; set; }
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        [StringLength(500)]
        public string? LastAddressBoarding { get; set; }
        [StringLength(500)]
        public string? LastAddressExit { get; set; }
        public DateTime Time { get; set; }


        public bool VIP
        {
            get => _vip;
            set
            {
                IsDirty = _vip != value;
                _vip = value;
            }
        }

        public bool Rejected { get; set; } = false;

        [StringLength(50)]
        public string Name { get; set; }
        public short VIP2 { get; set; }


    }
}
