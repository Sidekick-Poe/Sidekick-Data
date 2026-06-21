using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Trade.Models;

[Table("StaticItemCategories")]
[PrimaryKey(nameof(Game), nameof(Language), nameof(Id))]
public class TradeStaticItemCategory
{
    public int Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    public required string Id { get; set; }

    public string? Label { get; set; }
}
