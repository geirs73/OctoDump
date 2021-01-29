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
                // if (!(project.Name.Equals("Fis.Api.ClassStatus.Customers"))) continue;

                // Console.WriteLine($"Checking project {project.Name}");
                var projectVariableSet = spaceRepo.VariableSets.Get(project.VariableSetId);
                if (projectVariableSet == null) continue;

                foreach (var projectVariable in projectVariableSet.Variables)
                {
                    FindMatches(criteriumVariableSet, projectVariable.Name, projectVariable.Value ?? "", $"{project.Name}/ProjectVariable/{projectVariable.Name}");
                }

                var deploymentProcess = spaceRepo.DeploymentProcesses.Get(project.DeploymentProcessId);

                int stepCounter = 0;
                foreach (var step in deploymentProcess.Steps)
                {
                    stepCounter++;
                    int actionCounter = 0;
                    foreach (var action in step.Actions)
                    {
                        actionCounter++;
                        foreach (var prop in action.Properties)
                        {
                            FindMatches(criteriumVariableSet, prop.Key, prop.Value?.Value ?? "", $"/Project/{project.Name}/Step/{stepCounter}:{step.Name}/Actions/{actionCounter}:{action.Name}/{prop.Key}");
                        }
                    }

                    foreach (var prop in step.Properties)
                    {
                        FindMatches(criteriumVariableSet, prop.Key, prop.Value?.Value, $"/Project/{project.Name}/Step/{stepCounter}:{step.Name}/{prop.Key}");
                    }
                }
            }


            // var machines = repository.Machines.List();
            // var foo = machines;
        }
        private static void FindMatches(VariableSetResource criteriumVariableSet, string key, string value, string path)
        {
            foreach (var criteriumVariable in criteriumVariableSet.Variables)
            {
                if (value.Contains(criteriumVariable.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine($"{path}: {value}");
                }
            }
        }

    }
}
