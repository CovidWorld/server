namespace Sygic.Corona.Domain
{
    public class ClientInfo //Value object
    {
        public string Name { get; private set; }
        public string Integrator { get; private set; }
        public string Version { get; private set; }
        public string OperationSystem { get; private set; }

        public ClientInfo(string name, string integrator, string version, string operationSystem)
        {
            Name = name;
            Integrator = integrator;
            Version = version;
            OperationSystem = operationSystem;
        }
    }
}
