
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class CreateMerch
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }
        [Required]
        public Guid? ImageId { get; set; }
        [Required]
        public Guid EventId { get; set; }
    }
}
