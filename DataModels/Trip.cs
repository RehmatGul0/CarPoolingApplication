using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Trip
    {
        public long? RideId { get; set; }
        public long? PassengerId { get; set; }
        public long Id { get; set; }
        public DateTime? Time { get; set; }
        public int Seats { get; set; }

        public virtual Client Passenger { get; set; }
        public virtual Ride Ride { get; set; }
    }
}
