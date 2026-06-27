using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade;

[Table("TradeStats")]
[Index(nameof(Game), nameof(Language), nameof(Id), IsUnique = false)]
public class TradeStat
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    [MaxLength(256)]
    public string? OptionText { get; set; }

    [MaxLength(128)]
    public string? Type { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public TradeStatCategory? Category { get; set; }
}
