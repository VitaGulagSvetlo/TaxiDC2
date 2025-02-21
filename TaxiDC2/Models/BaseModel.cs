namespace TaxiDC2.Models
{
    
        public class BaseModel
        {
            public DateTime? DateModified { get; set; } = null;
            public DateTime? DateCreated { get; set; } = null;
            public DateTime? DateDeleted { get; set; } = null;
            public bool IsDirty { get; set; } = false;
            public bool IsDeleted { get; set; } = false;
            public string LastChangeUser { get; set; } = String.Empty;
            public string Memo { get; set; } = String.Empty;
        }
}
