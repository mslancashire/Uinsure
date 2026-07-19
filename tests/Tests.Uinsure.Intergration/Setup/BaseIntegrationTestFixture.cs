namespace Tests.Uinsure.Integration.Setup;

public abstract class BaseIntegrationTestFixture : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient _client;

    public BaseIntegrationTestFixture(CustomWebApplicationFactory application)
    {
        _client = application.CreateClient();
    }
}
