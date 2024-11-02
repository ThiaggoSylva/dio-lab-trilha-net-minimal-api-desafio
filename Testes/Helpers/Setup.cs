using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testes.Mocks;

namespace Testes.Helpers
{
    [TestClass]
    public class Setup
    {
        public static WebApplicationFactory<Startup> Factory {get; private set;}
        public static HttpClient Client {get; private set;}


        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder( builder => 
                {
                    builder.ConfigureServices(services => 
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAdministradorServico));
                        if(descriptor != null)
                            services.Remove(descriptor);
                        services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
                    });
                });
            Client = Factory.CreateClient();
        }
    }
}