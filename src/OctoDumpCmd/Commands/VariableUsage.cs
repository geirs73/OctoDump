using System;
using System.Linq;
using Octopus.Client;

namespace OctoDump.Commands
{
    public class VariableUsage
    {
        public IOctopusSpaceRepository SpaceRepository { get; set; }

        public VariableUsage(IOctopusSpaceRepository spaceRepository)
        {
            SpaceRepository = spaceRepository;
        }

        public void Search(string name)
        {

            var projects = SpaceRepository.Projects.GetAll().ToList();

            foreach (var project in projects)
            {
                var projectVariableSet = SpaceRepository.VariableSets.Get(project.VariableSetId);

                if (projectVariableSet == null) continue;

                foreach (var projectVariable in projectVariableSet.Variables)
                {
                    FindMatches(name, projectVariable.Name, projectVariable.Value ?? "", $"{project.Name}/ProjectVariable/{projectVariable.Name}");
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
                            FindMatches(name, prop.Key, prop.Value?.Value ?? "", $"/Project/{project.Name}/Step/{stepCounter}:{step.Name}/Actions/{actionCounter}:{action.Name}/{prop.Key}");
                        }
                    }

                    foreach (var prop in step.Properties)
                    {
                        FindMatches(name, prop.Key, prop.Value?.Value, $"/Project/{project.Name}/Step/{stepCounter}:{step.Name}/{prop.Key}");
                    }
                }
            }
        }
        private static void FindMatches(string name, string key, string value, string path)
        {
            if (value.Contains(name, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"{path}: {value}");
            }
        }

    }
}
