namespace Server
{
    using System.Reflection;
    using Castle.Facilities.Logging;
    using Castle.Windsor;
    using Doctors.Surgery.Contracts.V1;
    using NServiceBus;

    /*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, INeedInitialization
    {
        private const string Log4NetConfigFileName = "log4net.config";

        public static Assembly[] GetAssembliesToScan()
        {
            return new[]
            {
                typeof (MakeAppointmentV1).Assembly,
            };
        }

        private static IWindsorContainer InitialiseContainer()
        {
            using (var configuredContainer = new WindsorContainer())
            {
                using (var loggingFacility = new LoggingFacility(LoggerImplementation.Log4net, Log4NetConfigFileName))
                {
                    configuredContainer.AddFacility(loggingFacility);
                }
                return configuredContainer;
            }
        }

        public void Customize(BusConfiguration configuration)
        {
            var container = InitialiseContainer();
            configuration.AssembliesToScan(GetAssembliesToScan());
            configuration.UseContainer<WindsorBuilder>(b => b.ExistingContainer(container));
            configuration.UsePersistence<InMemoryPersistence>();
        }
    }
}
