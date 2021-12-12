
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.VM
{
    public class Gateway 
    {
        public Guid Id { set; get; }
        public DateTime DateOfCreate { set; get; }
        public string SerialNumber { set; get; }
        public string Name { set; get; }
        public string IPv4 { set; get; }
        public List<PeripheralDevice> PeripheralDevices { set; get; }
    }
}
