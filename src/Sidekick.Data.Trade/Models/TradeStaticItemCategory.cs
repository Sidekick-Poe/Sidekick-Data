using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sidekick.Data.Trade.Models;

[Table("StaticItemCategories")]
public class TradeStaticItemCategory
{
    [Key, Column(Order = 0)]
    public int Game { get; set; }

    [Key, Column(Order = 1), MaxLength(5)]
    public required string Language { get; set; }

    [Key, Column(Order = 2)]
    public required string Id { get; set; }

    public string? Label { get; set; }
}
