using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class Client
    {
        // Primary Key
        public virtual int ClientID { get; set; }
        public virtual int UserKey { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual int Age { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string SSN { get; set; }
        public virtual int Gender { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zip { get; set; }
        public virtual string HomePhone { get; set; }
        public virtual string Income { get; set; }
        public virtual int MarStat { get; set; }
        public virtual string Email { get; set; }

        public string FullName
        {
            get
            {
                string name = "";
                if (FirstName != null)
                    name = FirstName;
                if (LastName != null) 
                    name = name + " " + LastName;
                return name;
            }
        }
    }
}
