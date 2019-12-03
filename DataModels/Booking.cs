using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Booking
    {
        public long? UserId { get; set; }
        public long? RideId { get; set; }
        public long Id { get; set; }

        public virtual Ride Ride { get; set; }
        public virtual Client User { get; set; }
    }
}
