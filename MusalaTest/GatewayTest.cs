using Infrastructure.ViewModel.VM;
using Moq;
using MusalaTask.Controllers;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusalaTest
{
    public class GatewayTest 
    {

        [Fact]
        public void TestAdd()
        {
            var obj = new ResReqGatewayVM
            {
                DateOfCreate = DateTime.Now,
                IPv4 = "Test",
                Name = "Test",
                SerialNumber = "Test"

            };
            //

            var mockGateway = new Mock<IGatewayService>();
            mockGateway.Setup(p => p.Add(obj)).Returns(false);

            var controller = new GatewayController(mockGateway.Object);

            var result = controller.Add(obj);

            Assert.True(result);
        }

    }
}
