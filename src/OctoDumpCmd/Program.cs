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

            var variableSetResult = spaceRepo.LibraryVariableSets.FindByName(args[0]);
            var criteriumVariableSet = spaceRepo.VariableSets.Get(variableSetResult.VariableSetId);

            //var variables = vset.Variables;

            Console.WriteLine($"Looking for usages of variables from variable set {variableSetResult.Name} in {space.Name}");



            var projects = spaceRepo.Projects.GetAll().ToList();



            // foreach (var project in projects)
            // {
            //     var projectVariableSet = spaceRepo.VariableSets.Get(project.VariableSetId);
            //     foreach (var projectVariable in projectVariableSet.Variables) 
            //     {
            //         Console.WriteLine($"{project.Name}_{projectVariable.Name}");
            //     }
            // }

            foreach (var project in projects)
            {
                if (!(project.Name.Equals("Fis.Api.ClassStatus.Customers"))) continue;

                Console.WriteLine($"Checking project {project.Name}");
                var projectVariableSet = spaceRepo.VariableSets.Get(project.VariableSetId);
                if (projectVariableSet == null) continue;
                foreach (var variable in criteriumVariableSet.Variables)
                {

                    foreach (var pv in projectVariableSet.Variables)
                    {
                        Console.WriteLine($"does '{variable.Name}' exist in '{pv.Value ?? string.Empty}' ");
                    }
                    var matchingValueVariables = (from pv in projectVariableSet.Variables
                                                  where (pv.Value ?? "").Contains(variable.Name)
                                                  select pv).ToList();
                    foreach (var mv in matchingValueVariables)
                    {
                        var foo = mv.Value;
                    }
                }
                var deploymentProcess = spaceRepo.DeploymentProcesses.Get(project.DeploymentProcessId);
                foreach (var step in deploymentProcess.Steps)
                {
                    var props = new List<DeploymentStepProperties>();
                    foreach (var action in step.Actions)
                    {
                        

                    }
                    
                    foreach (var prop in props)
                    {
                        var foo2 = prop.Key;
                    }
                }
            }


            // var machines = repository.Machines.List();
            // var foo = machines;
        }
    }
}
