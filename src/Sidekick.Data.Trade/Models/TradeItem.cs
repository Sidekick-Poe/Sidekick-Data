using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("Items")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(Id))]
public class TradeItem
{
    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(256)]
    public required string Id { get; set; }

    [MaxLength(128)]
    public string? Discriminator { get; set; }

    [MaxLength(256)]
    public string? Name { get; set; }

    [MaxLength(256)]
    public string? Type { get; set; }

    [MaxLength(256)]
    public string? Text { get; set; }

    public bool IsUnique { get; set; }

    [MaxLength(128)]
    public string? CategoryId { get; set; }
}
