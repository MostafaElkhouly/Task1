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
    public class PeripheralDeviceService : IPeripheralDeviceService
    {

        private static readonly List<PeripheralDevice> peripheralDevices = new List<PeripheralDevice>();
        private readonly IMapper mapper;

        public PeripheralDeviceService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public bool Add(ResReqPeripheralDeviceVM req)
        {
            var value = mapper.Map<PeripheralDevice>(req);
            peripheralDevices.Add(value);
            return true;
        }

        public bool Delete(Guid Id)
        {
            var item = peripheralDevices.RemoveAll(e => e.Id == Id);
            return item > 0;
        }

        public List<ResReqPeripheralDeviceVM> GetAll()
        {
            var list = mapper.Map<List<ResReqPeripheralDeviceVM>>(peripheralDevices);
            return list;
        }

        public ResReqPeripheralDeviceVM GetById(Guid Id)
        {
            var item = peripheralDevices.Where(e => e.Id == Id).FirstOrDefault();
            var value = mapper.Map<ResReqPeripheralDeviceVM>(item);
            return value;
        }

        public bool Update(ResReqPeripheralDeviceVM req, Guid Id)
        {
            var item = peripheralDevices.Where(e => e.Id == Id).FirstOrDefault();
            var index = peripheralDevices.IndexOf(item);
            var value = mapper.Map<PeripheralDevice>(req);
            peripheralDevices[index] = value;
            return true;
        }
    }
}
