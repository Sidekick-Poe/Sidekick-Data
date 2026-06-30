using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sidekick.Data.BaseItems;
using Sidekick.Data.Ninja;
using Sidekick.Data.Trade;
using Sidekick.Data.Uniques;

namespace Sidekick.Data.ItemDefinitions;

[Table("ItemDefinitions")]
[Index(nameof(Game), nameof(Language))]
public class ItemDefinitionEntity
{
    [Key]
    public Guid SidekickId { get; set; }

    public GameType Game { get; set; }

    [MaxLength(5)]
    public required string Language { get; set; }

    public Guid? TradeItemId { get; set; }

    [ForeignKey(nameof(TradeItemId))]
    public TradeItem? TradeItem { get; set; }

    public Guid? BaseItemId { get; set; }

    [ForeignKey(nameof(BaseItemId))]
    public BaseItem? BaseItem { get; set; }

    public Guid? UniqueItemId { get; set; }

    [ForeignKey(nameof(UniqueItemId))]
    public UniqueItem? UniqueItem { get; set; }

    public Guid? NinjaExchangeItemId { get; set; }

    [ForeignKey(nameof(NinjaExchangeItemId))]
    public NinjaExchangeItem? NinjaExchangeItem { get; set; }

    public List<NinjaStashItem> NinjaStashItems { get; set; } = [];

    [MaxLength(512)]
    public string? NamePattern { get; set; }

    [MaxLength(512)]
    public string? TypePattern { get; set; }

    [MaxLength(512)]
    public string? TextPattern { get; set; }
}