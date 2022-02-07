using AutoMapper;
using Domain.Entities;
using Infrastructure.ViewModel.VM;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Data
{
    public class GatewayService : IGatewayService
    {
        private static readonly List<Gateway> gateways = new List<Gateway>();
        private static readonly List<PeripheralDevice> peripheralDevices = new List<PeripheralDevice>();

        private readonly IMapper mapper;

        public GatewayService(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public bool Add(ResReqGatewayVM req)
        {
            req.Id = Guid.NewGuid();

            if(req.PeripheralDevices != null && req.PeripheralDevices.Count > 10)
            {
                throw new ValidationException("Peripheral Devices must be less than or equal 10 devices");
            }

            req.PeripheralDevices = req.PeripheralDevices.Select(e =>
            {
                e.Id = Guid.NewGuid();
                e.GatewayId = req.Id;
                return e;
            }).ToList();

            //asdasd

            

            var value = mapper.Map<Gateway>(req);
            gateways.Add(value);

            if(req.PeripheralDevices != null && req.PeripheralDevices.Count > 0)
            {
                var perValue = mapper.Map<List<PeripheralDevice>>(req.PeripheralDevices);
                    peripheralDevices.AddRange(perValue);
            }

            return true;
        }

        public bool Delete(Guid Id)
        {
            var item = gateways.RemoveAll(e => e.Id == Id);
            peripheralDevices.RemoveAll(e => e.GatewayId == Id);
            return item > 0;
        }

        public List<ResReqGatewayVM> GetAll()
        {
            var list = mapper.Map<List<ResReqGatewayVM>>(gateways);
            list = list.Select(e =>
            {
                e.PeripheralDevices = mapper.Map<List<ResReqPeripheralDeviceVM>>(
                    peripheralDevices.Where(x => x.GatewayId == e.Id).ToList());
                return e;
            }).ToList();
            return list;
        }

        public ResReqGatewayVM GetById(Guid Id)
        {
            var item = gateways.Where(e => e.Id == Id).FirstOrDefault();
            var value = mapper.Map<ResReqGatewayVM>(item);
            value.PeripheralDevices
                = mapper.Map<List<ResReqPeripheralDeviceVM>>(
                    peripheralDevices.Where(e => e.GatewayId == value.Id).ToList());
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

        public bool AddPeripheralDevice(ResReqPeripheralDeviceVM req)
        {


            if(peripheralDevices.Where(e => e.GatewayId == req.GatewayId).Count() >= 10)
            {
                throw new ValidationException("Peripheral Devices must be less than or equal 10 devices");
            }

            var value = mapper.Map<PeripheralDevice>(req);
            value.Id = Guid.NewGuid();
            peripheralDevices.Add(value);
            return true;
        }

        public bool DeletePeripheralDeviceByUId(Guid Id)
        {
            var item  = peripheralDevices.RemoveAll(e => e.Id == Id);
            return item > 0;
        }

        public bool updateStatus( Guid Id)
        {
            var item = peripheralDevices.Where(e => e.Id == Id).FirstOrDefault();
            var indexOf = peripheralDevices.IndexOf(item);

            item.Status = !item.Status;
            peripheralDevices[indexOf] = item;
            return true;
        }


    }
}
