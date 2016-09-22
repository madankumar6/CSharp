using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Utils
{
    public class GeoPoint
    {
        public double X;
        public double Y;
    }

    public static class GeoPolygon
    {
        public static bool IsInside(Dictionary<string, string> myPts, string Xstr, string Ystr)
        {
            List<GeoPoint> polyPoints = myPts.Select(p => new GeoPoint() {
                X = double.Parse(p.Key),
                Y = double.Parse(p.Value)
            }).ToList();

            double X, Y;
            X = double.Parse(Xstr);
            Y = double.Parse(Ystr);

            return IsInside(polyPoints, X, Y);
        }

        public static bool IsInside(List<GeoPoint> myPts, double X, double Y)
        {
            int sides = myPts.Count() - 1;
            int j = sides - 1;
            bool pointStatus = false;
            for (int i = 0; i < sides; i++)
            {
                if (myPts[i].Y < Y && myPts[j].Y >= Y || myPts[j].Y < Y && myPts[i].Y >= Y)
                {
                    if (myPts[i].X + (Y - myPts[i].Y) / (myPts[j].Y - myPts[i].Y) * (myPts[j].X - myPts[i].X) < X)
                    {
                        pointStatus = !pointStatus;
                    }
                }
                j = i;
            }
            return pointStatus;
        }
    }
}