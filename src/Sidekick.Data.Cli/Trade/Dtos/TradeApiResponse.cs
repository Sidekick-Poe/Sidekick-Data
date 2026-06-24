namespace Sidekick.Data.Cli.Trade.Dtos;

internal sealed record TradeApiResponse<T>(List<T> Result);
