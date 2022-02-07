using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.VM
{
    public class ResReqPeripheralDeviceVM 
    {
        public Guid Id { set; get; }
        [Required]
        public int UID { set; get; }
        [Required]
        public string Vendor { set; get; }
        public bool Status { set; get; }
        [Required]
        public Guid GatewayId { set; get; }
    }
}
