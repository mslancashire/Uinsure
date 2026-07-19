using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tests.Uinsure.Integration.Services;
using Uinsure.Core.Repositories;

namespace Tests.Uinsure.Integration.Setup;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public IConfiguration? Configuration { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsetting.Integration.json")
            .Build();

            configBuilder.AddConfiguration(Configuration);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IPolicyRepository>();
            services.AddSingleton<IPolicyRepository, TestPolicyRepository>();
        });
    }
}
