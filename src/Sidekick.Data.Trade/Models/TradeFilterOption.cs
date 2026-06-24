using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("FilterOptions")]
[PrimaryKey(nameof(FilterUniqueId), nameof(Id))]
public class TradeFilterOption
{
    public Guid FilterUniqueId { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    public TradeFilter TradeFilter { get; set; } = null!;
}
