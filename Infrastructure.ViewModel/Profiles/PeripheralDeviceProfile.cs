using Domain.Entities;
using Infrastructure.ViewModel.Base;
using Infrastructure.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.Profiles
{
    public class PeripheralDeviceProfile : ProfileBase
    {
        public override void Request()
        {
            CreateMap<ResReqPeripheralDeviceVM, PeripheralDevice>();
        }

        public override void Response()
        {
            CreateMap<PeripheralDevice, ResReqPeripheralDeviceVM>();

        }
    }
}
