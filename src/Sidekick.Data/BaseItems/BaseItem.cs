using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data.ItemClasses;

namespace Sidekick.Data.BaseItems;

[Table("BaseItems")]
[Index(nameof(Game), nameof(Language))]
public class BaseItem
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    [MaxLength(128)]
    public required string Id { get; set; } // GraphQL ID

    [MaxLength(256)]
    public required string Name { get; set; }

    [MaxLength(256)]
    public string? ItemVisualIdentityId { get; set; }

    // Properties (Armour, ES, Evasion, Ward, etc.)
    public int ArmourMin { get; set; }
    public int ArmourMax { get; set; }
    public int EnergyShieldMin { get; set; }
    public int EnergyShieldMax { get; set; }
    public int EvasionMin { get; set; }
    public int EvasionMax { get; set; }
    public int WardMin { get; set; }
    public int WardMax { get; set; }
    public int PhysicalDamageMin { get; set; }
    public int PhysicalDamageMax { get; set; }
    public double AttacksPerSecond { get; set; }
    public double CriticalHitChance { get; set; }
    public int Block { get; set; }

    // Requirements
    public int DropLevel { get; set; }
    public int RequiresDexterity { get; set; }
    public int RequiresIntelligence { get; set; }
    public int RequiresStrength { get; set; }

    public Guid? ItemClassId { get; set; }

    [ForeignKey(nameof(ItemClassId))]
    public ItemClassEntity? ItemClass { get; set; }
}