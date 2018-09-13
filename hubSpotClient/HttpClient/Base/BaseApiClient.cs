using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace hubSpot.Client
{
    public class BaseApiClient : IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Fields with base server Url and required parameter for every http request
        /// </summary>
        private static string _base_url;
        private static string _access_token_param_name;
        private static string _access_token_value;
        private static string _access_token_url_segment { get { return _access_token_param_name + "=" + _access_token_value; } }
        /// <summary>
        /// Fields that contains runtime data about current or last Request
        /// </summary>
        private static string _current_request_url;
        private static ApiMethod _current_api_method;

        private HttpClient _httpClient;
        #endregion

        #region Init
        public BaseApiClient()
        {
            InitClient();
        }
        
        /// <summary>
        /// Initialize HttpClient
        /// </summary>
        private void InitClient()
        {
            SetConfiguration();

            _httpClient = new HttpClient();
            SetHeaders();

        }
        #endregion

        #region Public Fields
        /// <summary>
        /// Required properties for making API Calls
        /// </summary>
        public string BASE_URL { get { return _base_url; } }
        public string CURRENT_REQUEST_URL { get { return _current_request_url; } }
        public ApiMethod CURRENT_API_METHOD { get { return _current_api_method; } }
        #endregion

        #region Public Request Method
        /// <summary>
        /// Send a get request
        /// </summary>
        /// <param name="apiName">Name of the api Method</param>
        /// <param name="queryStr">query string with parameters</param>
        /// <returns>The HTTP response message if the call is successful</returns>
        public HttpResponseMessage Request(MethodName apiName, string queryStr = "")
        {
            var apiMethod = GetAPI(apiName, queryStr);

            var response = _httpClient.GetAsync(apiMethod.Url).Result;

            return response;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Get APIMethod for building Http Request
        /// </summary>
        /// <param name="apiName">Name of the api Method</param>
        /// <param name="queryStr">query string with parameters</param>
        /// <returns>ApiMethod object that contains data(url and parameters) to send Http Get Request</returns>
        private ApiMethod GetAPI(MethodName apiName, string queryStr = "")
        {
            var method = ApiMethods.Routes.Where(q => q.Key == apiName).FirstOrDefault();
            var query = HttpUtility.ParseQueryString(queryStr);
            method.Value.Url = method.Value.Url + _access_token_url_segment;
            for (int i = 0; i < query.Keys.Count; i++)
            {
                for (int j = 0; j < method.Value.ParamNames.Length; j++)
                {
                    if (query.Keys[i] == method.Value.ParamNames[j])
                    {
                        //Variable Parameter
                        if (method.Value.Url.Contains("{" + query.Keys[i] + "}"))
                        {
                            method.Value.Url = method.Value.Url.Replace("{" + query.Keys[i] + "}", query.Get(i));
                        }
                        //Query Parameter
                        else
                        {
                            var qVals = query.GetValues(i);
                            for (int q = 0; q < qVals.Length; q++)
                            {
                                method.Value.Url = method.Value.Url + "&" + query.Keys[i] + "=" + qVals[q];
                            }
                        }
                    }
                }
            }
            _current_request_url = method.Value.Url;

            return method.Value;
        }

        /// <summary>
        /// Set base Uri and headers of HttpClient
        /// </summary>
        private void SetHeaders()
        {
            _httpClient.BaseAddress = new Uri(_base_url);
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Initialize fields such as base Uri and access_token parameter
        /// </summary>
        private void SetConfiguration()
        {
            _base_url = ConfigurationManager.AppSettings["base_url"];
            _access_token_param_name = "?" + ConfigurationManager.AppSettings["access_token_param_name"];
            _access_token_value = ConfigurationManager.AppSettings["access_token_value"];
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                    _current_api_method = null;
                    _current_request_url = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseApiClient() {
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
