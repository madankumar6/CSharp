using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipsAndTraps
{
    public class BaseClass
    {
        public string Name { get; set; }
        public int Length { get; set; }

        public BaseClass()
        {
            InitName();
            Length = Name.Length;
        }

        public virtual void InitName()
        {
            Name = "Shivan";
        }
    }

    public class DerivedClass : BaseClass
    {
        public override void InitName()
        {
            Name = null;
        }
    }
}
