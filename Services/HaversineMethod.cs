using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPoolingApp.Services
{
    public static class HaversineMethod
    {
        public static double getDistance(double lat1, double lon1,
                        double lat2, double lon2)
        {
            // distance between latitudes and longitudes 
            double dLat = (Math.PI / 180) * (lat2 - lat1);
            double dLon = (Math.PI / 180) * (lon2 - lon1);

            // convert to radians 
            lat1 = (Math.PI / 180) * (lat1);
            lat2 = (Math.PI / 180) * (lat2);

            // apply formulae 
            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Pow(Math.Sin(dLon / 2), 2) *
                       Math.Cos(lat1) * Math.Cos(lat2);
            double rad = 6371;
            double c = 2 * Math.Asin(Math.Sqrt(a));
            return rad * c;
        }
    }
}
