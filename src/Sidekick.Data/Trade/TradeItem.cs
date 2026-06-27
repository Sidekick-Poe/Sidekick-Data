using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade;

[Table("TradeItems")]
public class TradeItem
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public string? Discriminator { get; set; }

    [MaxLength(256)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? Type { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    public bool IsUnique { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public TradeItemCategory? Category { get; set; }
}
