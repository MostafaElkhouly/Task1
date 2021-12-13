
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.VM
{
    public class ResReqGatewayVM 
    {
        public Guid Id { set; get; }
        public DateTime DateOfCreate { set; get; }
        [Required(ErrorMessage = "Please This SerialNumber Is Required")]
        public string SerialNumber { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", ErrorMessage = "The IPv4 Must be Valid")]
        public string IPv4 { set; get; }
      //  [MaxLength(0)]
        public List<ResReqPeripheralDeviceVM> PeripheralDevices { set; get; }
    }
}
