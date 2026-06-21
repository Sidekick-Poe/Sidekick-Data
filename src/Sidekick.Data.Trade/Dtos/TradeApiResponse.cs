using System.Text.Json.Serialization;

namespace Sidekick.Data.Trade.Dtos;

internal sealed record TradeApiResponse<T>(List<T> Result);
