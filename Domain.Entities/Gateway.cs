using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Gateway : EntityBase
    {
        public string SerialNumber { set; get; }
        public string Name { set; get; }
        public string IPv4 { set; get; }
    }
}
