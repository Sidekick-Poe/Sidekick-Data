using System.Text.RegularExpressions;

namespace Sidekick.Data.Cli.Pseudo;

public class PseudoPattern
(
    Regex regex,
    double multiplier = 1
)
{
    public Regex Pattern { get; set; } = regex;

    public double Multiplier { get; set; } = multiplier;
}
