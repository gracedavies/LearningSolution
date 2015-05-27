namespace Web.API.DependencyInjection
{
    using Castle.Core.Internal;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Doctors.Surgery.Contracts.V1;
    using NServiceBus;
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class CastleWindsorInstaller : IWindsorInstaller
    {
        //Installers that implement the IWindsorInstaller interface which has one method called Install. 
        // Usually you only have one single installer to install a close set of related services e.g. repositories
        // controllers etc. It better to have seperate smaller installers to make your code more readable and testable.
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            container.Register(
                Classes
                    .FromThisAssembly()
                    .BasedOn<IHttpController>()
                    .ConfigureFor<ApiController>(c => c.PropertiesIgnore(p => p.PropertyType.Is<HttpRequestMessage>()))
                    .LifestyleTransient());

            var busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.AssembliesToScan(typeof(MakeAppointmentV1).Assembly);
            busConfiguration.UsePersistence<InMemoryPersistence>();
            var bus = Bus.Create(busConfiguration).Start();
                

            container.Register(Component.For<IBus>().Instance(bus));
        }
    }
}