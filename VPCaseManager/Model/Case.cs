using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class Case
    {
        public virtual int Key { set; get; }
        public virtual int UserKey { set; get; }
        public virtual int ClientKey { set; get; }
        public virtual string ServerTag { set; get; }
        public virtual int Plan { set; get; }
        public virtual string CaseName { set; get; }
        public virtual string CaseNotes { set; get; }
        public virtual Double CaseDate { set; get; }
        public virtual string PlanName { set; get; }
        public virtual string PType { set; get; }
        public virtual string ServerVersion { set; get; }
        public virtual string WrapperVersion { set; get; }
    }
}
