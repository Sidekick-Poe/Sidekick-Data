using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("Filters")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(Id))]
public class TradeFilter
{
    public int Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    public required string Id { get; set; }

    public string? Text { get; set; }
    public string? Type { get; set; }
}
