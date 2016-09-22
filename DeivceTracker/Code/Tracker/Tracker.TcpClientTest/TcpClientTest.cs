using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Tracker.TcpClient.Config;
using System.Threading;
using Tracker.TcpClientTest.Data;

namespace Tracker.TcpClientTest
{
    public class TcpClientTest
    {
        public List<System.Net.Sockets.TcpClient> tcpclnts = new List<System.Net.Sockets.TcpClient>();
        public List<Thread> threads = new List<Thread>();

        public void Start()
        {
            try
            {
                ProtocolSection config = (ProtocolSection)ConfigurationManager.GetSection("ProtocolList");

                //for (int i = 0; i < config.ProtocolList.Count; i++)
                for (int i = 0; i < Convert.ToInt32(ConfigurationManager.AppSettings["TestClientCount"] ?? "10"); i++)
                {
                    //var ptr = config.ProtocolList[i];
                    var tHandler = new Thread((x) =>
                    {
                        //ProcessProtocols(ptr.Name, ptr.Payload);
                        Console.WriteLine("Starting for: " + x);
                        ProcessProtocols("TestClient " + x, (int)x);
                    });
                    tHandler.Start(i);
                    threads.Add(tHandler);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        public void Stop()
        {
            foreach (var t in threads)
            {
                t.Abort();
            }
            foreach (var client in tcpclnts)
            {
                client.Close();
            }
        }


        public void ProcessProtocols(string Name, int PayLoad)
        {
            try
            {
                //ISampleData sampleData = new GT03ASampleData(PayLoad);
                //ISampleData sampleData = new Gt00SampleData(PayLoad);
                ISampleData sampleData = new  Gt00SampleData(PayLoad);

                System.Net.Sockets.TcpClient tcpclnt = new System.Net.Sockets.TcpClient();
                Console.WriteLine(Name + ": Connecting.....");
                tcpclnt.Connect(ConfigurationManager.AppSettings["ServerIp"], Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"])); // use the ipaddress as in the server program
                tcpclnts.Add(tcpclnt);
                Console.WriteLine(Name + ": Connected");
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();

                int inc = 0;
                while (true)
                {
                    Random rnd = new Random();
                    Thread.Sleep(rnd.Next(1000, 5000));
                    //str = Console.ReadLine();
                    Console.WriteLine(Name + ": Transmitting..... " + PayLoad);
                    byte[] ba;

                    if (inc == 0)
                    {
                       
                        byte[] tba1 = {
                                         0x78, 0x78, 
                                         0x0D, 
                                         0x01, 0x03, 0x58,
                                         0x89, 0x90, 0x55,
                                         0x87, 0x57, 0x39,
                                         0x00, 0x1C, 0x55,
                                         0xE4, 0x0D, 0x0A
                                     };

                        ba = sampleData.getData("LoginData");
                    }
                    else if (inc == 1)
                    {

                        ba = sampleData.getData("HeartBeatData");
                    }
                    else if (inc == 2)
                    {
                        ba = sampleData.getData("LocationData");
                    }
                    else
                    {
                        ba = sampleData.getData("LocationData2");
                    }
                    stm.Write(ba, 0, ba.Length);
                    Console.WriteLine(Name + ": Transmitted: {0}", BitConverter.ToString(ba));
                    inc++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Name + ": Exception ");
                Console.WriteLine(ex);
            }
        }
    }
}
