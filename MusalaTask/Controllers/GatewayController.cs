using Infrastructure.ViewModel.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusalaTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayService service;

        public GatewayController(IGatewayService service)
        {
            this.service = service;
        }

        [HttpPost]
        public bool Add([FromBody] ResReqGatewayVM req)
        {
            return service.Add(req);
        }

        [HttpDelete("{Id}")]
        public bool Delete([FromRoute]Guid Id)
        {
            return service.Delete(Id);
        }

        [HttpGet]
        public List<ResReqGatewayVM> GetAll()
        {
            return service.GetAll();
        }

        [HttpGet("{Id}")]
        public ResReqGatewayVM GetById([FromRoute] Guid Id)
        {
            return service.GetById(Id);
        }

        [HttpPut("{Id}")]
        public bool Update([FromBody] ResReqGatewayVM req, [FromRoute] Guid Id)
        {
            return service.Update(req, Id);
        }

        [HttpDelete("DeletePeripheralDeviceById/{Id}")]
        public bool DeletePeripheralDeviceByUId([FromRoute]Guid Id)
        {
            return service.DeletePeripheralDeviceByUId(Id);
        }

        [HttpPost("AddPeripheralDevice")]
        public bool AddPeripheralDevice([FromBody]ResReqPeripheralDeviceVM req)
        {
            return service.AddPeripheralDevice(req);
        }

        [HttpPut("UpdateStatus/{Id}")]
        public bool UpdateStatus([FromRoute] Guid Id)
        {
            return service.updateStatus(Id);
        }
    }
}
