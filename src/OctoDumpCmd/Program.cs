using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using OctoDump.Commands;
using OctoDump.Infrastructure;


namespace OctoDump
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var rootCommand = new RootCommand("octodump");
                rootCommand.Handler = CommandHandler.Create<OctoDumpConfiguration>(c => Execute(c));
                OctoDumpCommandLine.CmdSymbols.ForEach(s => rootCommand.Add(s));

                var usageCmd = new Command("usage");
                rootCommand.Add(usageCmd);

                var vsuCmd = new Command("variable-set");
                vsuCmd.AddAlias("vs");
                VariableSetUsageCommandLine.CmdSymbols.ForEach(a => vsuCmd.Add(a));
                vsuCmd.Handler = CommandHandler.Create<VariableSetUsageConfiguration>(c => ExecuteVariableSetUsage(c));
                usageCmd.Add(vsuCmd);

                var vuCmd = new Command("variable");
                vuCmd.AddAlias("var");
                VariableUsageCommandLine.CmdSymbols.ForEach(a => vuCmd.Add(a));
                vuCmd.Handler = CommandHandler.Create<VariableUsageConfiguration>(c => ExecuteVariableUsage(c));
                usageCmd.Add(vuCmd);

                // int res = rootCommand.InvokeAsync(args).Result;
                int res = rootCommand.Invoke(args);
                if (res != 0) throw new Exception("Something went wrong when parsing options.");

                return res;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine($"Oops. Message: {exception.Message}");
                return 255;
            }
        }


        public static void ExecuteVariableSetUsage(VariableSetUsageConfiguration conf)
        {
            try
            {
                var conn = new OctoConnection();
                var spaceRepo = conn.GetSpaceRepository();
                var vsSearch = new VariableSetUsage(spaceRepo);
                vsSearch.Search(conf.VariableSetName);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error: " + Environment.NewLine +
                                        exception.Message);
            }
        }

        public static void ExecuteVariableUsage(VariableUsageConfiguration conf)
        {
            try
            {
                var conn = new OctoConnection();
                var spaceRepo = conn.GetSpaceRepository();
                var vsSearch = new VariableUsage(spaceRepo);
                vsSearch.Search(conf.VariableName);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error: " + Environment.NewLine +
                                        exception.ToString());
            }
        }


        private static void Execute(OctoDumpConfiguration o)
        {
            try
            {
                var conn = new OctoConnection();
                var spaceRepo = conn.GetSpaceRepository();
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error: " + Environment.NewLine +
                                        exception.Message);
            }
        }

    }
}
