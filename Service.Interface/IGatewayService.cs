using Infrastructure.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IGatewayService
    {

        public bool Add(ResReqGatewayVM req);
        public bool Delete(Guid Id);
        public List<ResReqGatewayVM> GetAll();
        public ResReqGatewayVM GetById(Guid Id);
        public bool Update(ResReqGatewayVM req, Guid Id);
        public bool updateStatus(Guid Id);
        public bool AddPeripheralDevice(ResReqPeripheralDeviceVM req);
        public bool DeletePeripheralDeviceByUId(Guid Id);

    }
}
