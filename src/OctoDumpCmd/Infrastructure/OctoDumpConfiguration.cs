using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace OctoDump.Infrastructure
{
    public class OctoDumpConfiguration
    {
        public string Server { get; set; }
        public string ApiKey { get; set; }
        public string SpaceName { get; set; }
    }


    public static class OctoDumpCommandLine
    {
        public static List<Symbol> CmdSymbols
        {
            get
            {
                var symbols = new List<Symbol>()
                {
                    new Option<string>( "--space-name", getDefaultValue: () => "Default", "Name of space to query"),
                    new Option<string>( "--apiKey", "Octopus ApiKey"),
                    new Option<string>( "--server", "Octopus server url")
                };
                return symbols;

            }
        }
    }

}
