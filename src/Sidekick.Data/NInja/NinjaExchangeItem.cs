using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Data.Ninja;

[Table("NinjaExchangeItems")]
[Index(nameof(Game), nameof(Type))]
public class NinjaExchangeItem
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(64)]
    public required string Type { get; set; }

    [MaxLength(256)]
    public string? Id { get; set; }

    [MaxLength(256)]
    public string? DetailsId { get; set; }
}
