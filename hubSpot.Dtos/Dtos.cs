using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hubSpot.Dtos
{
    public class ContactsJson
    {
        public List<ContactJson> Contacts { get; set; }
    }
    public class CompaniesJson
    {
        public List<CompanyJson> Companies { get; set; }
    }

    public class ContactJson
    {
        [JsonProperty("vid")]
        public string Vid { get; set; }

        [JsonProperty("properties")]
        public ContactPropertiesJson Properties { get; set; }
        public CompanyJson Associated_company { get; set; }
    }

    public partial class CompanyJson
    {
        [JsonProperty("companyId")]
        public long CompanyId { get; set; }

        [JsonProperty("properties")]
        public CompanyPropertiesJson Properties { get; set; }
    }

    public class CompanyPropertiesJson
    {
        public PopertyJson name { get; set; }
        public PopertyJson website { get; set; }
        public PopertyJson city { get; set; }
        public PopertyJson state { get; set; }
        public PopertyJson zip { get; set; }
        public PopertyJson phone { get; set; }
    }

    public class ContactPropertiesJson
    {
        public PopertyJson firstname { get; set; }
        public PopertyJson associatedcompanyid { get; set; }
        public PopertyJson lastmodifieddate { get; set; }
        public PopertyJson lastname { get; set; }
    }

    public class PopertyJson
    {
        public string value { get; set; }
    }
}
