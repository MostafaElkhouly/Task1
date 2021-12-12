using AutoMapper;
using Domain.Entities;
using Infrastructure.ViewModel.VM;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Data
{
    public class GatewayService : IGatewayService
    {
        private static readonly List<Gateway> gateways = new List<Gateway>();
        private readonly IMapper mapper;

        public GatewayService(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public bool Add(ResReqGatewayVM req)
        {
            var value = mapper.Map<Gateway>(req);
            gateways.Add(value);
            return true;
        }

        public bool Delete(Guid Id)
        {
            var item = gateways.RemoveAll(e => e.Id == Id);
            return item > 0;
        }

        public List<ResReqGatewayVM> GetAll()
        {
            var list = mapper.Map<List<ResReqGatewayVM>>(gateways);
            return list;
        }

        public ResReqGatewayVM GetById(Guid Id)
        {
            var item = gateways.Where(e => e.Id == Id).FirstOrDefault();
            var value = mapper.Map<ResReqGatewayVM>(item);
            return value;
        }

        public bool Update(ResReqGatewayVM req, Guid Id)
        {
            var item = gateways.Where(e => e.Id == Id).FirstOrDefault();
            var index = gateways.IndexOf(item);
            var value = mapper.Map<Gateway>(req);
            gateways[index] = value;
            return true;
        }
    }
}
