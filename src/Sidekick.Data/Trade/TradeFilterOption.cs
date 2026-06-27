using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sidekick.Data.Trade;

[Table("TradeFilterOptions")]
public class TradeFilterOption
{
    [Key]
    public Guid SidekickId { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    public Guid FilterId { get; set; }

    [ForeignKey(nameof(FilterId))]
    public TradeFilter TradeFilter { get; set; } = null!;
}
