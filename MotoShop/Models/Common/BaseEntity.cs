namespace MotoShop.Models.Common
{
    public abstract class BaseEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
