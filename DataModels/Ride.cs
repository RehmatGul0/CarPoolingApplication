using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Ride
    {
        public Ride()
        {
            Location = new HashSet<Location>();
            Trip = new HashSet<Trip>();
        }

        public long? VehicleId { get; set; }
        public int Seats { get; set; }
        public int Fee { get; set; }
        public string StartLoc { get; set; }
        public string EndLoc { get; set; }
        public DateTime? Time { get; set; }
        public long Id { get; set; }

        public virtual Vehicle Vehicle { get; set; }
        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<Trip> Trip { get; set; }
    }
}
