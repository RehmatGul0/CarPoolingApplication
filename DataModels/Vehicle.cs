using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Ride = new HashSet<Ride>();
        }

        public long? UserId { get; set; }
        public int Model { get; set; }
        public string Plate { get; set; }
        public string Description { get; set; }
        public long Id { get; set; }

        public virtual Client User { get; set; }
        public virtual ICollection<Ride> Ride { get; set; }
    }
}
