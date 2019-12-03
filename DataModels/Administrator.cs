using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Administrator
    {
        public long? AuthId { get; set; }
        public long Id { get; set; }

        public virtual Authdetail Auth { get; set; }
    }
}
