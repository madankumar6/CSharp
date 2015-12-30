using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplicitInterface
{
    class Program
    {
        //This sample demonstrates how to explicitly implement interface members and how to access those members from interface instances. 
        static void Main(string[] args)
        {
            // Declare a class instance "myBox":
            Box myBox = new Box(30.0f, 20.0f);

            // Declare an interface instance "myDimensions":
            IDimensions myDimensions = (IDimensions)myBox;

            // Print out the dimensions of the box:
            /* The following commented lines would produce compilation 
               errors because they try to access an explicitly implemented
               interface member from a class instance:                   */
            //System.Console.WriteLine("Length: {0}", myBox.Length());
            //System.Console.WriteLine("Width: {0}", myBox.Width());
            /* Print out the dimensions of the box by calling the methods 
               from an instance of the interface:                         */
            System.Console.WriteLine("Length: {0}", myDimensions.Length());
            System.Console.WriteLine("Width: {0}", myDimensions.Width());

            // Declare an instance of the English units interface:
            IEnglishDimensions eDimensions = (IEnglishDimensions)myBox;
            // Declare an instance of the metric units interface:
            IMetricDimensions mDimensions = (IMetricDimensions)myBox;
            // Print dimensions in English units:
            System.Console.WriteLine("Length(in): {0}", eDimensions.Length());
            System.Console.WriteLine("Width (in): {0}", eDimensions.Width());
            // Print dimensions in metric units:
            System.Console.WriteLine("Length(cm): {0}", mDimensions.Length());
            System.Console.WriteLine("Width (cm): {0}", mDimensions.Width());

            Console.Read();
        }
    }
}
