using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class View
    {
        public virtual int Key { get; set; }
        public virtual int UserKey { get; set; }
        public virtual string ViewName { get; set; }
        public virtual string Columns { get; set; }
        public virtual bool DeleteOK { get; set; }
        public virtual string Product { get; set; }
        public virtual int PlanCode { get; set; }
    }
}
