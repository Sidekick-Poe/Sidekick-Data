using Sidekick.Data.Stats;

namespace Sidekick.Data.Cli.Stats.Hooks;

public abstract class BaseHook
{
    public virtual void OnAfterGameBuild(List<StatDefinition> statDefinitions) {}
    public virtual void OnAfterTradeBuild(List<StatDefinition> statDefinitions) {}
}
