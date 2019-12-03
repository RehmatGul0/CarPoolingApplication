using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Route
    {
        public long? RideId { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public long Id { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
