using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker.TcpClientTest.Data
{
    public interface ISampleData
    {
        byte[] getData(string dataType);
    }
}
