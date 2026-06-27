using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Ninja.Models;

[Table("StashTradeStats")]
public class NinjaStashTradeStat
{
    [Key]
    public Guid UniqueId { get; set; }

    [MaxLength(256)]
    public string? StashItemDetailsId { get; set; }

    [MaxLength(512)]
    public string? Mod { get; set; }

    public int? Value { get; set; }

    [MaxLength(256)]
    public string? Option { get; set; }

    [ForeignKey(nameof(StashItemDetailsId))]
    public NinjaStashItem? StashItem { get; set; }
}