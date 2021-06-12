using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using AirportService.Config;

namespace AirportService
{
    internal static class Program
    {
        private const string BaseAddress = "http://localhost:8080/AirportService";

        public static void Main()
        {
            var host = new ServiceHost(typeof(AirportService), new Uri(BaseAddress));
            var binding = new BasicHttpBinding {MaxReceivedMessageSize = int.MaxValue};
            host.AddServiceEndpoint(typeof(IAirportService), binding, "");
            var smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>() ?? new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);
            host.Description.Behaviors.Add(new AirportServiceBehavior());
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
                MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            try
            {
                host.Open();
                Console.WriteLine("Airport Service started");
                Console.WriteLine("Press ENTER to close the service");
                Console.ReadLine();
                host.Close();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem: " + commProblem.Message);
                Console.Read();
            }
        }
    }
}