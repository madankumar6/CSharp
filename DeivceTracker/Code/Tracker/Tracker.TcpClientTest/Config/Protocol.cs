using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracker.TcpClient.Config
{
    public class ProtocolSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ProtocolCollection ProtocolList
        {
            get { return (ProtocolCollection)this[""]; }
            set { this[""] = value; }
        }
    }
    public class ProtocolCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProtocolElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ((ProtocolElement)element).Name;
        }

        public new ProtocolElement this[int index]
        {
            get
            {
                return this.OfType<ProtocolElement>().ToList()[index];
            }
        }

        public new ProtocolElement this[string elementName]
        {
            get
            {
                return this.OfType<ProtocolElement>().FirstOrDefault(item => item.Name == elementName);
            }
        }
    }

    public class ProtocolElement : ConfigurationElement
    {
        //Make sure to set IsKey=true for property exposed as the GetElementKey above
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; }
        }

        [ConfigurationProperty("Payload", IsRequired = true)]
        public string Payload
        {
            get { return (string)base["Payload"]; }
            set { base["Payload"] = value; }
        }
    } 

    //public class ProtocolSection : ConfigurationSection
    //{
    //    // Create a "Name" element.
    //    [ConfigurationProperty("Protocols")]
    //    public ProtocolElement[] Protocols
    //    {
    //        get
    //        {
    //            return (ProtocolElement[])this["Protocols"];
    //        }
    //        set
    //        { this["Protocols"] = value; }
    //    }
    //}

    //public class ProtocolElement : ConfigurationElement
    //{
    //    // Create a "Name" element.
    //    [ConfigurationProperty("Name")]
    //    public string Name
    //    {
    //        get
    //        {
    //            return (string)this["Name"];
    //        }
    //        set
    //        { this["Name"] = value; }
    //    }

    //    // Create a "Value" attribute.
    //    [ConfigurationProperty("Payload")]
    //    public string Payload
    //    {
    //        get
    //        {
    //            return (string)this["Payload"];
    //        }
    //        set
    //        {
    //            this["Payload"] = value;
    //        }
    //    }
    //}
}
