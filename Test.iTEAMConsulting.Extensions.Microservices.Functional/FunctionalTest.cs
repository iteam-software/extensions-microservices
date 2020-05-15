using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Xunit;

namespace Test.iTEAMConsulting.Extensions.Microservices.Functional
{
  public class FunctionalTest : IClassFixture<TestApp>, IDisposable
  {
    protected HttpClient Client { get; }
    private IServiceScope Scope;

    public FunctionalTest(TestApp factory)
    {
      Client = factory.CreateClient();
      Scope = factory.Services.CreateScope();
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          Client.Dispose();
          Scope.Dispose();

          Scope = null;
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~FunctionalTests()
    // {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion
  }
}
