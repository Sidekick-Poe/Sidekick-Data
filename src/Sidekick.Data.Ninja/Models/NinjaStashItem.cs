using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data;

namespace Sidekick.Data.Ninja.Models;

[Table("StashItems")]
[Index(nameof(Game), nameof(Type))]
public class NinjaStashItem
{
    [Key]
    public Guid UniqueId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(64)]
    public required string Type { get; set; }

    [MaxLength(256)]
    public string? DetailsId { get; set; }

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
}
