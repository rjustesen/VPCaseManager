using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class Plan
    {
        public virtual int Key { get; set; }
        public virtual int PlanID { get; set; }
        public virtual string Description { get; set; }
        public virtual string PType { get; set; }
        public virtual string PlanKey { get; set; }
        public virtual bool Active { get; set; }
        public virtual int PlanOrder { get; set; }
        public virtual bool Available { get; set; }
        public virtual bool PopUp { get; set; }

    }
}
