using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PeripheralDevice : EntityBase
    {
        [Required]
        public int UID { set; get; }
        [Required]
        public string Vendor { set; get; }
        public bool Status { set; get; }
        public Guid GatewayId { set; get; }
        [ForeignKey("GatewayId")]
        public Gateway Gateway { set; get; }
    }
}
