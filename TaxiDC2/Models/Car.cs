﻿using System.ComponentModel.DataAnnotations;

namespace TaxiDC2.Models
{
    public class Car : BaseModel
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

        public string FullName => $"{CarType} / {Color} / RZ: {Rz}";

        public bool Active { get; set; } = true;
    }

}
