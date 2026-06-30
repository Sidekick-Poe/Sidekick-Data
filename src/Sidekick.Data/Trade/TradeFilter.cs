using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade;

[Table("TradeFilters")]
[Index(nameof(Game), nameof(Language), nameof(Id))]
public class TradeFilter
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

    public bool? Hidden { get; set; }

    public bool? FullSpan { get; set; }

    public bool? HalfSpan { get; set; }

    public bool? MinMax { get; set; }

    public bool? Sockets { get; set; }

    [MaxLength(256)]
    public string? Tip { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public TradeFilterCategory Category { get; set; } = null!;

    public List<TradeFilterOption> Options { get; set; } = new();
}
