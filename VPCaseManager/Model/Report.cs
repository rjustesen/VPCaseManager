using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class Report
    {
        public virtual int Key { get; set; }
        public virtual int PlanKey { get; set; }
        public virtual string ReportName { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Active { get; set; }
        public virtual int Rtype { get; set; }
        public virtual string Topic { get; set; }
        public virtual int Value { get; set; }

    }
}
