using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("StatOptions")]
[PrimaryKey(nameof(TradeStatUniqueId), nameof(Id))]
public class TradeStatOption
{
    public Guid TradeStatUniqueId { get; set; }

    public int Id { get; set; }

    [MaxLength(256)]
    public required string Text { get; set; }

    public TradeStat TradeStat { get; set; } = null!;
}
