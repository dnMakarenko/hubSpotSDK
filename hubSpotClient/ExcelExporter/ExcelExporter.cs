using hubSpot.Dtos;
using hubSpot.ExcelExporter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hubSpot.Client
{
    public sealed class ExcelExporter
    {
        #region Private Config Fields
        private readonly string export_directory;
        private readonly string export_excel_file_format;
        private readonly string export_excel_sheet_name;
        #endregion

        #region Init
        public ExcelExporter()
        {
            export_directory = ConfigurationManager.AppSettings["export_excel_directory"];
            export_excel_file_format = ConfigurationManager.AppSettings["export_excel_file_format"];
            export_excel_sheet_name = ConfigurationManager.AppSettings["export_excel_sheet_name"];
        }
        #endregion

        #region Public Export Method
        public bool Export(List<ContactJson> contacts)
        {
            try
            {
                var dir_path = GetDirectory();

                using (ExcelExport eX = new ExcelExport())
                {
                    eX.AddSheet(export_excel_sheet_name, contacts.Select(q => new
                    {
                        Vid = q.Vid,
                        Firstname = q.Properties.firstname.value,
                        Lastname = q.Properties.lastname.value,
                        Lastmodifieddate = q.Properties.lastmodifieddate.value,
                        Associatedcompanyid = q.Properties.associatedcompanyid.value,
                        Name = q.Associated_company.Properties.name.value,
                        City = q.Associated_company.Properties.city.value,
                        Website = q.Associated_company.Properties.website.value,
                        Phone = q.Associated_company.Properties.phone.value,
                        State = q.Associated_company.Properties.state.value,
                        Zip = q.Associated_company.Properties.zip.value
                    }));

                    eX.ExportTo(Path.Combine(dir_path, string.Format("{0} {1}.{2}", DateTime.Now.Ticks, export_excel_sheet_name, export_excel_file_format)));

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't export data to Excel file. See inner excpetion."), ex);
            }
        }
        #endregion

        #region Private Helper Methods
        private string GetDirectory()
        {
            var dir_path = Path.Combine(export_directory);
            try
            {
                if (!Directory.Exists(dir_path))
                {
                    try
                    {
                        Directory.CreateDirectory(dir_path);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Couldn't create directory. Path: '{0}'", dir_path), e);
                    }
                }
                return dir_path;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Couldn't get directory. Path: '{0}'", dir_path), ex);
            }
        }
        #endregion
    }
}
