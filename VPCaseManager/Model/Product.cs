using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager.Model
{
    public class Product
    {
        public virtual int Key { get; set; }
        public virtual string ServerID { get; set; }
        public virtual string Description { set; get; }
        public virtual string LOB { set; get; }
        public virtual string PType { set; get; }
        public virtual int ProdOrder { set; get; }
        public virtual bool Active { set; get; }
        public virtual List<Plan> Plans { set; get; }

        public Product()
        {
            Plans = new List<Plan>();
        }

    }
}
