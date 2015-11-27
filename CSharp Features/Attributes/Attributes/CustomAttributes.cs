using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    class CustomAttributes
    {
    }
    // The IsTested class is a user-defined custom attribute class.
    // It can be applied to any declaration including - types (struct, class, enum, delegate)
    // and members (methods, fields, events, properties, indexers). It is used with no arguments.
    public class IsTestedAttribute : Attribute
    {
        public override string ToString()
        {
            return "Is Tested";
        }
    }

    // The AuthorAttribute class is a user-defined attribute class. It can be applied to classes and struct declarations only.
    // It takes one unnamed string argument (the author's name). It has one optional named argument Version, which is of type int.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class AuthorAttribute : Attribute
    {
        private string name;
        private int version;

        public string Name 
        { 
            get
            {
                return this.name;
            }
        }

        public int Version
        {
            get
            {
                return this.version;
            }
            set
            {
                version = value;
            }
        }
        public AuthorAttribute(string name)
        {
            this.name = name;
            this.version = 0;
        }

        public override string ToString()
        {
            string value = "Author : " + Name;
            if (version != 0)
            {
                value += " Version : " + Version.ToString();
            }
            return value;
        }
    }

    [Author("Madankumar", Version = 1)]
    class Order
    {
        public int OrderNo { get; set; }
        public int Amount { get; set; }
    }

    [Author("Madankumar")]
    class OrderAccount
    {
        ArrayList orderList = new ArrayList();

        [IsTested]
        public void AddOrder(Order order)
        {
            orderList.Add(order);
        }
    }
}
