using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("Stats")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(Id))]
public class TradeStat
{
    public int Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    public required string Id { get; set; }

    public string? Text { get; set; }
    public string? Type { get; set; }

    public List<TradeStatOption> Options { get; set; } = new();

    public string? CategoryId { get; set; }
    public TradeStatCategory? Category { get; set; }
}
