using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{/// <summary>
 /// Summary description for User.
 /// </summary>
    [Serializable]
    public class User
    {
        // Primary Key
        public virtual int UserKey { set; get; }
        public virtual string UserID { set; get; }
        public virtual string PassWord { set; get; }
        public virtual int AgentKey { set; get; }
        public virtual int GroupKey { set; get; }
        public virtual string UserName { set; get; }
        // Fields that come from the associated user object
        public virtual string CompanyName { set; get; }
        public virtual string AgencyAddress { set; get; }
        public virtual string Phone { set; get; }
        public virtual string AgentName { set; get; }

        public User()
        {
          
        }

    }
}
