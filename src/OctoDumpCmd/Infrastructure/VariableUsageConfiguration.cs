using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace OctoDump.Infrastructure
{
    public class VariableUsageConfiguration : OctoDumpConfiguration
    {
        public string VariableName { get; set; }

    }

    public static class VariableUsageCommandLine
    {
        public static List<Symbol> CmdSymbols
        {
            get
            {
                var symbols = (new List<Symbol>()
                {
                    new Argument<string>( "variable-name", "Find variables in used other places")
                });
                symbols.AddRange(OctoDumpCommandLine.CmdSymbols);
                return symbols;
            }
        }
    }
}
