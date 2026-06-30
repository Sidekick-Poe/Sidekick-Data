using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data.BaseItems;

namespace Sidekick.Data.Uniques;

[Table("UniqueItems")]
[Index(nameof(Game), nameof(Language))]
public class UniqueItem
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public required string Name { get; set; }

    [MaxLength(512)]
    public string? Image { get; set; }

    public Guid? BaseItemId { get; set; }

    [ForeignKey(nameof(BaseItemId))]
    public BaseItem? BaseItem { get; set; }
}