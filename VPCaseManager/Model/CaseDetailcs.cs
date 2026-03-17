using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class CaseDetail
    {
        public virtual int Key { get; set; }
        public virtual int CaseKey { get; set; }
        public virtual string Topic_Name { get; set; }
        public virtual int Selector { get; set; }
        public virtual int Value { get; set; }
        public virtual int ValueType { get; set; }
        public virtual string Blobject { get; set; }

    }
}
