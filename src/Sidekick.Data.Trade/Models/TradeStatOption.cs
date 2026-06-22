using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("StatOptions")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(StatId), nameof(Id))]
public class TradeStatOption
{
    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string StatId { get; set; }

    public int Id { get; set; }

    [MaxLength(256)]
    public required string Text { get; set; }
}
