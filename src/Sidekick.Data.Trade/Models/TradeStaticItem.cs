using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("StaticItems")]
[Index(nameof(Game), nameof(Language), nameof(Id))]
public class TradeStaticItem
{
    [Key]
    public Guid UniqueId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    [MaxLength(256)]
    public string? Image { get; set; }

    [MaxLength(128)]
    public string? CategoryId { get; set; }
}
