using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker.Common.Model
{
    public enum AlertMediumType
    {
        Email = 0,
        SMS = 1
    }


    public class ATFPosition
    {
        public string Lat { get; set; }
        public string Lang { get; set; }
        public float Distance { get; set; }
        public int ListOrder { get; set; }
    }
}
