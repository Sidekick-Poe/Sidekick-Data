using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sidekick.Data.StatsInvariant;

[Table("StatsInvariantLogbookBoss")]
public class StatsInvariantLogbookBoss
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(128)]
    public required string StatId { get; set; }
}
