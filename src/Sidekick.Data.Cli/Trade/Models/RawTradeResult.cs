namespace Sidekick.Data.Cli.Trade.Models;

public class RawTradeResult<TResult>
{
    public required TResult Result { get; init; }
}