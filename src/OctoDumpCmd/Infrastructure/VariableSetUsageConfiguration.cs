using System.Collections.Generic;
using System.CommandLine;

namespace OctoDump.Infrastructure
{
    public class VariableSetUsageConfiguration : OctoDumpConfiguration
    {
        public string VariableSetName { get; set; }

    }

    public static class VariableSetUsageCommandLine
    {
        public static List<Symbol> CmdSymbols
        {
            get
            {
                var symbols = (new List<Symbol>()
                {
                    new Argument<string>("variable-set-name", "Find variables in this set used other places")
                });
                symbols.AddRange(OctoDumpCommandLine.CmdSymbols);
                return symbols;
            }
        }
    }
}
