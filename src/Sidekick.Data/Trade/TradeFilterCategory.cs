using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade;

[Table("TradeFilterCategories")]
[Index(nameof(Game), nameof(Language), nameof(Id))]
public class TradeFilterCategory
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public string? Id { get; set; }

    [MaxLength(256)]
    public string? Label { get; set; }

    public List<TradeFilter> Filters { get; set; } = [];
}
