using iTEAMConsulting.Extensions.Microservices;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Xunit;

namespace Test.iTEAMConsulting.Extensions.Microservices.Functional
{
  public class AppInsightsMiddlewareTests
  {

    [Fact]
    public async Task MiddlewareTest()
    {
      // Arrange
      using var host = await new HostBuilder()
        .ConfigureAppConfiguration(config => config
          .AddJsonFile("appsettings.json"))
        .ConfigureWebHost(builder => builder
          .UseTestServer()
          .ConfigureServices((context, services) =>
          {
            var options = context.Configuration
              .GetSection("Microservice")
              .Get<MicroserviceOptions>();
            services.AddMicroservice(options);
          })
          .Configure(app => app.UseMicroservice()))
        .StartAsync();

      // Act
      var server = host.GetTestServer();
      var client = server.Services.GetRequiredService<TelemetryClient>();

      // Assert
      Assert.Equal("12345678-9012-3456-7890-123456789012", client.TelemetryConfiguration.InstrumentationKey);
    }
  }
}
