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
using Newtonsoft.Json;
using hubSpot.Dtos;
using hubSpot.ExcelExporter;
using System.IO;


namespace hubSpot.Client
{
    public class HubSpotClient : IDisposable
    {
        #region Private Fields
        private static ApiClient _client;
        private static ExcelExporter _excelExporter;
        #endregion

        #region Init
        public HubSpotClient()
        {
            _client = new ApiClient();
            _excelExporter = new ExcelExporter();
        }
        #endregion

        #region Operations
        public ContactJson[] GetContats(DateTime modifiedOnOrAfter)
        {
            try
            {
                var ticks = modifiedOnOrAfter.ToUniversalTime().Ticks;
                var contacts = GetContacts().Contacts.Where(q => Convert.ToInt64(q.Properties.lastmodifieddate.value) <= ticks).ToArray();
                if (contacts.Length > 0)
                {
                    foreach (var contact in contacts)
                    {
                        var company = GetCompany(contact.Properties.associatedcompanyid.value);
                        if (company != null)
                        {
                            contact.Associated_company = company;
                        }
                    }
                }
                return contacts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExportToExcel(List<ContactJson> contacts)
        {
            try
            {
                if (contacts.Count > 0)
                {
                    var export_result = _excelExporter.Export(contacts);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods
        private ContactsJson GetContacts()
        {
            try
            {
                var contacts_request = _client.GetContacts("count=100&property=associatedCompanyId&property=lastname&property=firstname");
                var contacts_response = contacts_request.Content.ReadAsStringAsync().Result;
                var contacts = JsonConvert.DeserializeObject<ContactsJson>(contacts_response);

                return contacts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CompanyJson GetCompany(string companyId)
        {
            try
            {
                var company_request = _client.GetCompany(string.Format("companyId={0}&count=100&property=name&property=website&property=city&property=state&property=zip&property=phone", companyId));
                var company_response = company_request.Content.ReadAsStringAsync().Result;
                var company = JsonConvert.DeserializeObject<CompanyJson>(company_response);

                return company;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidateToken()
        {
            var token_response = _client.ValidateToken();
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
                    _client.Dispose();
                    _excelExporter = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HubSpotClient() {
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
