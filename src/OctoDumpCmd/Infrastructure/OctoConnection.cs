using Octopus.Client;

namespace OctoDump.Infrastructure
{
    public class OctoConnection
    {
        public OctoConnection(string server, string apiKey)
        {
            Server = server;
            ApiKey = apiKey;
        }

        public string Server { get; set; }
        public string ApiKey { get; set; }

        public IOctopusSpaceRepository GetSpaceRepository(string spaceName = "Default")
        {
            var octoEndpoint = new OctopusServerEndpoint(Server, ApiKey);
            var client = new OctopusClient(octoEndpoint);
            var sysRepo = client.ForSystem();

            var space = sysRepo.Spaces.FindByName(spaceName);
            var spaceRepo = client.ForSpace(space);
            return spaceRepo;
        }


    }
}
