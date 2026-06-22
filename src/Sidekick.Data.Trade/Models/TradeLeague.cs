using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data.Leagues;

namespace Sidekick.Data.Trade.Models;

[Table("Leagues")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(Id))]
public class TradeLeague
{
    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    public LeagueRealm Realm { get; set; }
}
