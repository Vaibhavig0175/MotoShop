using MotoShop.Data;
using MotoShop.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace MotoShop.Models
{
    public class Notification: BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Url { get; set; }

        public bool IsRead { get; set; }

        public NotificationType Type { get; set; }
    }
}
