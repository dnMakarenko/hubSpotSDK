using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hubSpot.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new HubSpotClient())
            {
                try
                {
                    Console.WriteLine("Start to retrieve contacts");
                    var contacts = client.GetContats(DateTime.Now);

                    Console.WriteLine("\nContacts Ids and CompanyIds:");
                    foreach (var item in contacts)
                    {
                        Console.WriteLine(string.Format("ContactId: '{0}' CompanyId: '{1}'", item.Vid, item.Associated_company.CompanyId));
                    }

                    Console.WriteLine("\nStart exporting to excel");
                    client.ExportToExcel(contacts.ToList());
                    Console.WriteLine("\nDone!");

                }
                catch (Exception e)
                {
                    Console.WriteLine("Couldn't save data to excel! See inner exception: {0}", e);
                }

                Console.ReadLine();
            }
        }
    }
}
