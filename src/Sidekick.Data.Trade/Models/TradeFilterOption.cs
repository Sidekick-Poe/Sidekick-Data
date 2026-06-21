using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("FilterOptions")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(FilterGroupId), nameof(FilterId), nameof(Id))]
public class TradeFilterOption
{
    public int Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string FilterGroupId { get; set; }

    [MaxLength(128)]
    public required string FilterId { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }
}
