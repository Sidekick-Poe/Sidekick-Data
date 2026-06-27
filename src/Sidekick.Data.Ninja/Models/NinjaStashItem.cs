using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data;

namespace Sidekick.Data.Ninja.Models;

[Table("StashItems")]
public class NinjaStashItem
{
    [Key]
    [MaxLength(256)]
    public string? DetailsId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(64)]
    public required string Type { get; set; }

    [MaxLength(512)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? BaseType { get; set; }

    public bool? Corrupted { get; set; }

    public int? GemLevel { get; set; }

    public int? GemQuality { get; set; }

    public int? Links { get; set; }

    public int? LevelRequired { get; set; }

    [MaxLength(256)]
    public string? Variant { get; set; }

    public List<NinjaStashTradeStat> TradeStats { get; set; } = new();

    public List<NinjaStashMutatedStat> MutatedStats { get; set; } = new();
}