using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxiDC2.Models
{
    [ObservableObject]
    public partial class Car : BaseModel
    {
        public Guid IdCar { get; set; }

        [StringLength(50)]
        [Required]
        public string CarType { get; set; }

        [StringLength(50)]
        [Required]
        public string Rz { get; set; }

        [StringLength(50)]
        [Required]
        public string Color { get; set; }

        public string FullName => $"{CarType} barva {Color} RZ : {Rz}";
    }

}
