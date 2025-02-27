using System.ComponentModel.DataAnnotations;

namespace TaxiDC2.Models
{
	public class Customer : BaseModel
	{
		public Guid IdCustomer { get; set; }
		[StringLength(20)]
		public string PhoneNumber { get; set; }
		[StringLength(500)]
		public string LastAddressBoarding { get; set; }
		[StringLength(500)]
		public string LastAddressExit { get; set; }
		public DateTime? Time { get; set; }
		public bool Rejected { get; set; } = false;
		[StringLength(50)]
		public string Name { get; set; }
		// ReSharper disable once InconsistentNaming
		public short VIP2 { get; set; } = 0;
	}
}
