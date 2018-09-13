using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hubSpot.Client
{
    /// <summary>
    /// Class that represents base Http Get Request
    /// </summary>
    public sealed class ApiMethod
    {
        public string Url { get; set; }
        public string[] ParamNames { get; set; }

        public ApiMethod(string url, string[] paramNames = null)
        {
            Url = url;
            if (paramNames != null)
                ParamNames = paramNames;
        }
    }

    /// <summary>
    /// IDictionary with Method names and urls
    /// </summary>
    public struct ApiMethods
    {
        public static IDictionary<MethodName, ApiMethod> Routes
        {
            get
            {
                var routes = new Dictionary<MethodName, ApiMethod>
                {
                    { MethodName.validateToken, new ApiMethod(ConfigurationManager.AppSettings["validateToken_url"], null) },
                    { MethodName.getContacts, new ApiMethod(ConfigurationManager.AppSettings["getContacts_url"], ConfigurationManager.AppSettings["getContacts_params"].Split(',')) },
                    { MethodName.getCompanyById, new ApiMethod(ConfigurationManager.AppSettings["getCompanyById_url"], ConfigurationManager.AppSettings["getCompanyById_params"].Split(',')) }
                };

                return routes;
            }
        }
    }

    /// <summary>
    /// enum with API Methods signatures
    /// </summary>
    public enum MethodName
    {
        validateToken,
        getContacts,
        getCompanyById
    }
}
