using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Test.iTEAMConsulting.Extensions.Microservices.Functional
{
  public class StatusTests : FunctionalTest
  {
    private readonly IServiceProvider _services;

    public StatusTests(TestApp factory) : base(factory)
    {
      _services = factory.Services;
    }

    [Fact]
    public async void PingStatus()
    {
      // Act
      var response = await Client.GetAsync("/status");

      // Assert
      response.EnsureSuccessStatusCode();
    }

    [Fact]
    public void InstrumentationKey()
    {
      // Arrange
      var client = _services.GetRequiredService<TelemetryClient>();

      // Assert
      Assert.Equal("12345678-9012-3456-7890-123456789012", client.TelemetryConfiguration.InstrumentationKey);
    }
  }
}
