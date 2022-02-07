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
    public class GatewayProfile : ProfileBase
    {
        public override void Request()
        {
            CreateMap<ResReqGatewayVM, Gateway>();
        }

        public override void Response()
        {
            CreateMap<Gateway, ResReqGatewayVM>();

        }
    }
}
