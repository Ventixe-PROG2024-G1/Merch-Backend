
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Domain;

public class MerchEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public Guid? ImageId { get; set; }
    public Guid EventId { get; set; }
}
