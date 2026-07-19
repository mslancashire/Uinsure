using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

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
        });
    }
}
