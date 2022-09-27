using iTEAMConsulting.Extensions.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Test.iTEAMConsulting.Extensions.Microservices.Functional
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      var options = Configuration.GetSection("Microservice").Get<MicroserviceOptions>();
      services.AddMicroservice(options);
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMicroservice();
    }
  }

  public class TestApp : WebApplicationFactory<Startup>
  {
    protected override IHostBuilder CreateHostBuilder()
    {
      return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(config =>
        {
          config.AddJsonFile("appsettings.Local.json");
        })
        .ConfigureWebHostDefaults(builder =>
        {
          builder.UseStartup<Startup>();
        });
    }
  }
}
