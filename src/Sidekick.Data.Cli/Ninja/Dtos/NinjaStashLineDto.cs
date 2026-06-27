namespace Sidekick.Data.Cli.Ninja.Dtos;

public sealed class NinjaStashLineDto
{
    public string? DetailsId { get; init; }
    public string? Name { get; init; }
    public string? BaseType { get; init; }
    public bool? Corrupted { get; init; }
    public int? GemLevel { get; init; }
    public int? GemQuality { get; init; }
    public int? Links { get; init; }
    public int? LevelRequired { get; init; }
    public string? Variant { get; init; }
    public List<NinjaModTextDto>? TradeInfo { get; init; }
    public List<NinjaModIdDto>? MutatedModifiers { get; init; }
}