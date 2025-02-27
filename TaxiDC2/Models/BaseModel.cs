using System.ComponentModel;

namespace TaxiDC2.Models
{
	public class BaseModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public DateTime? DateModified { get; set; } = null;
		public DateTime? DateCreated { get; set; } = null;
		public DateTime? DateDeleted { get; set; } = null;
		public bool IsDirty { get; set; } = false;
		public bool IsDeleted { get; set; } = false;
		public string LastChangeUser { get; set; } = string.Empty;
		public string Memo { get; set; } = string.Empty;
	}
}
