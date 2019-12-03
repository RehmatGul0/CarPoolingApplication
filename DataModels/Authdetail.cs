using System;
using System.Collections.Generic;

namespace CarPoolingApp.DataModels
{
    public partial class Authdetail
    {
        public Authdetail()
        {
            Administrator = new HashSet<Administrator>();
            Client = new HashSet<Client>();
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public long Id { get; set; }
        public string Salt { get; set; }

        public virtual ICollection<Administrator> Administrator { get; set; }
        public virtual ICollection<Client> Client { get; set; }
    }
}
