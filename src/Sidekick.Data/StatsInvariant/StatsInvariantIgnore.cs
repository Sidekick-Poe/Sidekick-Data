using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data;

namespace Sidekick.Data.StatsInvariant;

[Table("StatsInvariantIgnore")]
public class StatsInvariantIgnore
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(128)]
    public required string StatId { get; set; }
}
