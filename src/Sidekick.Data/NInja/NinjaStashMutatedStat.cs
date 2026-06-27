using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sidekick.Data.Ninja;

[Table("NinjaStashMutatedStats")]
public class NinjaStashMutatedStat
{
    [Key]
    public Guid SidekickId { get; set; }

    [MaxLength(512)]
    public string? Text { get; init; }

    public bool Optional { get; init; }

    public Guid StashItemId { get; set; }

    [ForeignKey(nameof(StashItemId))]
    public NinjaStashItem? StashItem { get; set; }
}