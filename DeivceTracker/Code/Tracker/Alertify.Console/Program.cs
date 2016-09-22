using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alertify.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int ProcessorWaitTime = 1000;
            ProcessorWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessorWaitTime"] ?? "1000");

            AlertifyMain alertifyProcessor = new AlertifyMain(1);
            while (true)
            {
                System.Console.WriteLine("alertifyProcessor.Start()");
                alertifyProcessor.Start();
                System.Console.WriteLine("alertifyProcessor.ProcessFiredAlerts()");
                alertifyProcessor.ProcessFiredAlerts();
                System.Console.WriteLine("alertifyProcessor Finished");
                System.Console.WriteLine();
                System.Console.WriteLine();
                Thread.Sleep(ProcessorWaitTime);
            }
            System.Console.ReadLine();
        }
    }
}
