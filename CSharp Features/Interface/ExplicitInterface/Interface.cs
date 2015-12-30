using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplicitInterface
{
    interface IDimensions
    {
        float Length();
        float Width();
    }

    // Declare the English units interface:
    interface IEnglishDimensions
    {
        float Length();
        float Width();
    }

    // Declare the metric units interface:
    interface IMetricDimensions
    {
        float Length();
        float Width();
    }


    public class Box : IDimensions, IEnglishDimensions, IMetricDimensions
    {
        float lengthInches;
        float widthInches;

        public Box(float lengthInches, float widthInches)
        {
            this.lengthInches = lengthInches;
            this.widthInches = widthInches;
        }

        // Explicit interface member implementation: 
        float IDimensions.Length()
        {
            return lengthInches;
        }

        // Explicit interface member implementation:
        float IDimensions.Width()
        {
            return widthInches;
        }

        float IEnglishDimensions.Length()
        {
            return lengthInches;
        }

        float IEnglishDimensions.Width()
        {
            return widthInches;      
        }

        // Explicitly implement the members of IMetricDimensions:
        float IMetricDimensions.Length()
        {
            return lengthInches * 2.54f;
        }

        float IMetricDimensions.Width()
        {
            return widthInches * 2.54f;
        }
    }
}
