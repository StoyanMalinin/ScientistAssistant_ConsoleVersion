using System;
using System.Collections.Generic;
using System.Text;

namespace ScientistAssistant_ConsoleVersion
{
    class EarthPoint
    {
        public double latitude { get; set; }
        public double longtitude { get; set; }

        public object additionalInfo { get; set; }

        public EarthPoint() { }
        public EarthPoint(double latitude, double longtitude)
        {
            this.latitude = latitude;
            this.longtitude = longtitude;
        }
        public EarthPoint(double latitude, double longtitude, object additionalInfo)
        {
            this.latitude = latitude;
            this.longtitude = longtitude;
            this.additionalInfo = additionalInfo;
        }
        public EarthPoint(Datasets.EONET.DatasetClasses.Event e)
        {
            this.additionalInfo = e;
            this.latitude = e.getAverageCoordinate(1);
            this.longtitude = e.getAverageCoordinate(0);
        }
    }

    static class Earth
    {
        public static double R = 6371;
    }
   
    static class GeometryUtils
    {
        private static double getPointDist(EarthPoint A, EarthPoint B)
        {
            double dLatitude = Math.Abs(A.latitude - B.latitude)*(Math.PI/180);
            double dLongtitude = Math.Abs(A.longtitude - B.longtitude) * (Math.PI / 180);

            double angleRaw = Math.Sin(dLatitude / 2) * Math.Sin(dLatitude / 2) +
                              Math.Cos(A.latitude * (Math.PI / 180)) * Math.Cos(B.latitude* (Math.PI / 180)) *
                              Math.Sin(dLongtitude / 2) * Math.Sin(dLongtitude / 2);

            double angle = 2 * Math.Atan2(Math.Sqrt(angleRaw), Math.Sqrt(1 - angleRaw));
            return angle * Earth.R;
        }

        public static Tuple<double, EarthPoint, EarthPoint> findClosestPoints(List <EarthPoint> all)
        {
            double minDist = double.MaxValue;
            Tuple<double, EarthPoint, EarthPoint> ans = null;

            for(int i = 0;i<all.Count;i++)
            {
                for(int j = i+1;j<all.Count;j++)
                {
                    double d = getPointDist(all[i], all[j]);
                    if(d<minDist)
                    {
                        minDist = d;
                        ans = new Tuple<double, EarthPoint, EarthPoint>(minDist, all[i], all[j]);
                    }
                }
            }

            return ans;
        }

        public static Tuple<double, EarthPoint, EarthPoint> findFurthestPoints(List<EarthPoint> all)
        {
            double maxDist = double.MinValue;
            Tuple<double, EarthPoint, EarthPoint> ans = null;

            for (int i = 0; i < all.Count; i++)
            {
                for (int j = i + 1; j < all.Count; j++)
                {
                    double d = getPointDist(all[i], all[j]);
                    if (d > maxDist)
                    {
                        maxDist = d;
                        ans = new Tuple<double, EarthPoint, EarthPoint>(maxDist, all[i], all[j]);
                    }
                }
            }

            return ans;
        }
    }
}
