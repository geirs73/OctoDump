using System;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctoDump.Commands
{
    public class VariableSetUsage
    {
        public IOctopusSpaceRepository SpaceRepository { get; set; }

        public VariableSetUsage(IOctopusSpaceRepository spaceRepository)
        {
            SpaceRepository = spaceRepository;
        }

        public void Search(string name)
        {

            var variableSetResult = SpaceRepository.LibraryVariableSets.FindByName(name);

            var criteriumVariableSet = SpaceRepository.VariableSets.Get(variableSetResult.VariableSetId);

            var projects = SpaceRepository.Projects.GetAll().ToList();

            foreach (var project in projects)
            {
                var projectVariableSet = SpaceRepository.VariableSets.Get(project.VariableSetId);

                if (projectVariableSet == null) continue;

                foreach (var projectVariable in projectVariableSet.Variables)
                {
                    FindMatches(criteriumVariableSet, projectVariable.Name, projectVariable.Value ?? "", $"{project.Name}/ProjectVariable/{projectVariable.Name}");
                }

                var deploymentProcess = SpaceRepository.DeploymentProcesses.Get(project.DeploymentProcessId);


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
