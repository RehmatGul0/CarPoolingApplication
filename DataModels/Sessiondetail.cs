using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Sessiondetail
    {
        public bool IsActive { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long? AuthId { get; set; }
        public string SessionId { get; set; }
    }
}
