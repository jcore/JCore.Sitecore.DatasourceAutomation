using Glass.Mapper.Sc;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Car.Foundation.Models
{
    public class RegisterDI : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceCollection.AddTransient<ISitecoreContext>(provider => new SitecoreContext());
            serviceCollection.AddTransient<IGlassHtml>(provider => new GlassHtml(serviceProvider.GetService<ISitecoreContext>()));
            serviceCollection.AddTransient<IControllerScContext>(provider => new ControllerScContext(serviceProvider.GetService<IGlassHtml>()));
        }
    }
}