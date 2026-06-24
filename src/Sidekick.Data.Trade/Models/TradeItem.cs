using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("Items")]
[Index(nameof(Game), nameof(Language), nameof(CategoryId), IsUnique = false)]
public class TradeItem
{
    [Key]
    public Guid UniqueId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    public Guid CategoryId { get; set; }

    [MaxLength(128)]
    public string? Discriminator { get; set; }

    [MaxLength(256)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? Type { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    public bool IsUnique { get; set; }

    [ForeignKey("CategoryId")]
    public TradeItemCategory? Category { get; set; }
}
