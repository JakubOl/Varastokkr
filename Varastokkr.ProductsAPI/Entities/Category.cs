using System.ComponentModel.DataAnnotations;

namespace Varastokkr.ProductAPI.Entities;

internal class Category
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string Description { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
