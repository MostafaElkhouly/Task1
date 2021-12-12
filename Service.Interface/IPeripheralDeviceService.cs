using Infrastructure.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPeripheralDeviceService
    {
        public bool Add(ResReqPeripheralDeviceVM req);
        public bool Delete(Guid Id);
        public List<ResReqPeripheralDeviceVM> GetAll();
        public ResReqPeripheralDeviceVM GetById(Guid Id);
        public bool Update(ResReqPeripheralDeviceVM req, Guid Id);
    }
}
