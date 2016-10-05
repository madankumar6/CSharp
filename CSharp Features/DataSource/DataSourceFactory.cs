using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class DataSourceFactory
    {

        public DataSourceFactory()
        {
            GetCustomesDataFromXml("DataSource.CustomersData.xml");
            CreateLists();
        }

        public string GetCustomesDataFromXml(string resourceName)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string xmlData = "";
            using (var reader = new StreamReader(_assembly.GetManifestResourceStream(resourceName)))
            {
                xmlData = reader.ReadToEnd();
            }

            return xmlData;
        }

        public StreamReader GetCustomesDataXmlStream(string resourceName)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();

            return new StreamReader(_assembly.GetManifestResourceStream(resourceName));
        }

        private void CreateLists()
        {
        }

    }
}
