using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Preferences
    {
        public long? UserId { get; set; }
        public long Id { get; set; }
        public string Gender { get; set; }
        public bool? Notification { get; set; }

        public virtual Client User { get; set; }
    }
}
