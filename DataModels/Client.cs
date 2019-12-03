using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Client
    {
        public Client()
        {
            Preferences = new HashSet<Preferences>();
            Trip = new HashSet<Trip>();
            Vehicle = new HashSet<Vehicle>();
        }

        public string Name { get; set; }
        public string Phone { get; set; }
        public int Rating { get; set; }
        public long? AuthId { get; set; }
        public long Id { get; set; }
        public string Gender { get; set; }
        public bool? IsDriver { get; set; }

        public virtual Authdetail Auth { get; set; }
        public virtual ICollection<Preferences> Preferences { get; set; }
        public virtual ICollection<Trip> Trip { get; set; }
        public virtual ICollection<Vehicle> Vehicle { get; set; }
    }
}
