using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctoDumpCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var server = System.Environment.GetEnvironmentVariable("OctoServer");
            var apiKey = System.Environment.GetEnvironmentVariable("OctoApiKey");

            var octoEndpoint = new OctopusServerEndpoint(server, apiKey);
            var client = new OctopusClient(octoEndpoint);
            var sysRepo = client.ForSystem();

            var space = sysRepo.Spaces.FindByName("Default");
            var spaceRepo = client.ForSpace(space);

            var vset = spaceRepo.LibraryVariableSets.FindByName(args[0]);
            var vset2 = spaceRepo.VariableSets.Get(vset.VariableSetId);

            //var variables = vset.Variables;

            Console.WriteLine($"Looking for usages of variables from variable set {vset.Name} in {space.Name}");

            var projects = spaceRepo.Projects.GetAll().ToList();
            foreach (var project in projects)
            {

                Console.WriteLine($"Checking project {project.Name}");
                var projectVariableSet = spaceRepo.VariableSets.Get(project.VariableSetId);
                if (projectVariableSet == null) continue;
                foreach (var variable in vset2.Variables)
                {
                    var matchingValueVariables = (from v in projectVariableSet.Variables
                                                 where (v.Value ?? "").Contains(variable.Name) 
                                                 select v).ToList();
                    foreach (var mv in matchingValueVariables)
                    { 
                        var foo = mv.Value;
                    }



                }
            }

            // var machines = repository.Machines.List();
            // var foo = machines;
        }
    }
}
