using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.ItemClasses;

[Table("ItemClasses")]
[Index(nameof(Game), nameof(Language), nameof(Id))]
public class ItemClassEntity
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Name { get; set; }

    public ItemClass Type { get; set; }
}
