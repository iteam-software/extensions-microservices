using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace iTEAMConsulting.Extensions.Microservices
{
  public static class ApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseMicroservice(this IApplicationBuilder app)
    {
      app.UseHttpsRedirection();

      app.UseRouting();

      app.Use(async (context, next) =>
      {
        var telemetry = context.RequestServices.GetRequiredService<TelemetryClient>();
        var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("MicroserviceMiddleware");

        var requestTelemetry = new RequestTelemetry
        {
          Name = $"{context.Request.Method} {context.Request.Path}"
        };

        if (context.Request.Headers.ContainsKey("Operation-Id"))
        {
          var upstreamOperationId = context.Request.Headers["Operation-Id"];
          requestTelemetry.Context.Operation.ParentId = upstreamOperationId;
        }

        var operation = telemetry.StartOperation(requestTelemetry);

        try
        {
          await next.Invoke();
        }
        catch (Exception e)
        {
          requestTelemetry.Success = false;
          telemetry.TrackException(e);
          throw e;
        }
        finally
        {
          if (context.Items.ContainsKey("Operation-Id"))
          {
            requestTelemetry.Context.Operation.Id = context.Items["Operation-Id"].ToString();
          }

          if (context.User != null && context.User.Identity.IsAuthenticated)
          {
            requestTelemetry.Context.User.Id = context.User.Identity.Name;
          }

          if (context.Response != null)
          {
            requestTelemetry.ResponseCode = context.Response.StatusCode.ToString();
            requestTelemetry.Success = context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299;
          }
          else
          {
            requestTelemetry.Success = false;
          }

          telemetry.StopOperation(operation);
        }
      });

      app.Map("/status", app => app.Run(async context =>
      {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
      }));

      app.UseAuthentication();

      app.UseAuthorization();

      return app;
    }
  }
}
