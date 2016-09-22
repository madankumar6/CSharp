using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Tracker.Common
{
    public static class Config
    {
        public static Dictionary<string, string> GetProtocolsNPorts()
        {
            Dictionary<string, string> pNP = new Dictionary<string, string>();

            try
            {
                string protocolsNPorts = ConfigurationManager.AppSettings["ProtocolsNPorts"] ?? "";
                //pNP = protocolsNPorts.Split(',').ToList().Select(p => { 
                //    { p.Split('|')[0], p.Split('|')[1]}
                //}).ToList();

                foreach (var p in protocolsNPorts.Split(',').ToList())
                {
                    pNP.Add(p.Split('|')[0], p.Split('|')[1]);
                }
            }
            catch (Exception ex)
            {
            }

            return pNP;
        }

    }
}
