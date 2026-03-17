using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class Agent
    {
        public virtual int Key { set; get; }
        public virtual string AgentID { set; get; }
        public virtual string LastName { set; get; }
        public virtual string FirstName { set; get; }
        public virtual string MiddleName { set; get; }
        public virtual string Title { set; get; }
        public virtual string Company { set; get; }
        public virtual string AgencyCode { set; get; }
        public virtual string Email { set; get; }
        public virtual string Address1 { set; get; }
        public virtual string Address2 { set; get; }
        public virtual string City { set; get; }
        public virtual int State { set; get; }
        public virtual string Zip { set; get; }
        public virtual string DialNumber { set; get; }

        public Agent()
        {

        }
    }
}
