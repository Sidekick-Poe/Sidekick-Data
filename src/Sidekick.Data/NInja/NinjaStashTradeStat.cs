using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sidekick.Data.Ninja;

[Table("NinjaStashTradeStats")]
public class NinjaStashTradeStat
{
    [Key]
    public Guid SidekickId { get; set; }

    [MaxLength(512)]
    public string? Mod { get; init; }

    public int? Min { get; init; }

    public int? Max { get; init; }

    [MaxLength(512)]
    public string? Option { get; init; }

    public Guid StashItemId { get; set; }

    [ForeignKey(nameof(StashItemId))]
    public NinjaStashItem StashItem { get; set; } = null!;
}