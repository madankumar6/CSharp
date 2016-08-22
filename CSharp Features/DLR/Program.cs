using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DLR
{
    public class Person
    {
        public string Name { get; set; }

        public void Speak()
        {
            Console.WriteLine("Hello..");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //DynamicCall();
            //DynamicCOM();
            //XmlLINQ();
            //ExpandoXml();
        }

        private static void ExpandoXml()
        {
            var doc = XDocument.Load("Employees.xml").AsExpando();

            foreach (var employee in doc.Employees)
            {
                Console.WriteLine(employee.FirstName);
            }
        }

        private static void XmlLINQ()
        {
            var doc = XDocument.Load("Employees.xml");

            foreach (var item in doc.Element("Employees").Elements("Employee"))
            {
                Console.WriteLine(item.Element("FirstName").Value);
            }
        }

        private static void DynamicCall()
        {
            object o = GetASpeaker();
            o.GetType().GetMethod("Speak").Invoke(o, null);
            dynamic person = GetASpeaker();
            person.Speak();
        }

        private static void DynamicCOM()
        {
            Type excelType = Type.GetTypeFromProgID("Excel.Application");
            dynamic excel = Activator.CreateInstance(excelType);

            excel.Visible = true;
            excel.Workbooks.Add();

            dynamic worksheet = excel.ActiveSheet;

            Process[] processes = Process.GetProcesses();

            for (int i = 0; i < processes.Length; i++)
            {
                worksheet.Cells[i + 1, "A"] = processes[i].ProcessName;
                worksheet.Cells[i + 1, "B"] = processes[i].Threads.Count;
            }
        }

        private static object GetASpeaker()
        {
            return new Person();
        }
    }
}
