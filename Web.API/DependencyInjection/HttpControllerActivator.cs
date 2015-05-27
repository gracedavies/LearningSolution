namespace Web.API.DependencyInjection
{
    using Castle.Windsor;
    using System;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    public class HttpControllerActivator : IHttpControllerActivator
    {
        private readonly IWindsorContainer container;

        public HttpControllerActivator(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
           return (IHttpController)this.container.Resolve(controllerType, new { request }); 
        }
    }
}