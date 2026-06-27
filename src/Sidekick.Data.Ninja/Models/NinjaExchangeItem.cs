using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data;

namespace Sidekick.Data.Ninja.Models;

[Table("ExchangeItems")]
[Index(nameof(Game), nameof(Type))]
public class NinjaExchangeItem
{
    [Key]
    public Guid UniqueId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(64)]
    public required string Type { get; set; }

    [MaxLength(256)]
    public string? Id { get; set; }

    [MaxLength(256)]
    public string? DetailsId { get; set; }

    [MaxLength(256)]
    public string? CurrencyId { get; set; }

    [MaxLength(256)]
    public string? CurrencyDetailsId { get; set; }

    public long? ChaosValue { get; set; }
}
