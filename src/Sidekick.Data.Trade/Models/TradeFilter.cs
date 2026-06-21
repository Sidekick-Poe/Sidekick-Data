using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("Filters")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(FilterGroupId), nameof(Id))]
public class TradeFilter
{
    public int Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string FilterGroupId { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }
    public bool? Hidden { get; set; }
    public bool? FullSpan { get; set; }
    public bool? HalfSpan { get; set; }
    public bool? MinMax { get; set; }
    public bool? Sockets { get; set; }
    public string? Tip { get; set; }

    public List<TradeFilterOption> Options { get; set; } = new();
}
