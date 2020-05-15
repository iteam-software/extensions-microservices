using System;
using Xunit;

namespace Test.iTEAMConsulting.Extensions.Microservices.Functional
{
  public class StatusTests : FunctionalTest
  {
    public StatusTests(TestApp factory) : base(factory)
    {
    }

    [Fact]
    public async void PingStatus()
    {
      // Act
      var response = await Client.GetAsync("/status");

      // Assert
      response.EnsureSuccessStatusCode();
    }
  }
}
