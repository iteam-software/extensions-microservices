using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace iTEAMConsulting.Extensions.Microservices
{
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Add microservice service package to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="instrumentationKey"></param>
    /// <returns></returns>
    public static IServiceCollection AddMicroservice(this IServiceCollection services, MicroserviceOptions options)
    {
      if (string.IsNullOrEmpty(options.InstrumentationKey))
      {
        throw new ArgumentNullException(nameof(options.InstrumentationKey));
      }

      if (string.IsNullOrEmpty(options.DirectoryId))
      {
        throw new ArgumentNullException(nameof(options.DirectoryId));
      }

      if (string.IsNullOrEmpty(options.Audience))
      {
        throw new ArgumentNullException(nameof(options.Audience));
      }

      services.AddMvc();

      services.AddApplicationInsightsTelemetry();

      services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(config =>
        {
          config.Authority = MicroserviceDefaults.RootAuthority + options.DirectoryId;
          config.Audience = options.Audience;
        });

      return services;
    }
  }
}
