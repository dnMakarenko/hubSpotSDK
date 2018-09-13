using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace hubSpot.Client
{
    public sealed class ApiClient : IDisposable
    {
        #region Private Fields
        private BaseApiClient _client;
        #endregion

        #region Init
        public ApiClient()
        {
            _client = new BaseApiClient();
        }
        #endregion

        #region Api Methods
        public HttpResponseMessage ValidateToken(string query = "")
        {
            return _client.Request(MethodName.validateToken, query);
        }

        public HttpResponseMessage GetContacts(string query)
        {
            return _client.Request(MethodName.getContacts, query);
        }

        public HttpResponseMessage GetCompany(string query)
        {
            return _client.Request(MethodName.getCompanyById, query);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ApiClient() {
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
