using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("Items")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(Id), nameof(Discriminator))]
public class TradeItem
{
    public int Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    public required string Id { get; set; }

    public string? Discriminator { get; set; }

    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Text { get; set; }
    public bool IsUnique { get; set; }

    public string? CategoryId { get; set; }
    public TradeItemCategory? Category { get; set; }
}