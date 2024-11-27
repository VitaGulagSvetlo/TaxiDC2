using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDC2.Models;

namespace TaxiDC2.Models
{
    public class Car : BaseModel
    {
        [Key]
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
