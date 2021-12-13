using Infrastructure.ViewModel.VM;
using Moq;
using MusalaTask.Controllers;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusalaTest
{
    public class GatewayTest 
    {
        private readonly Guid StaticId;

        public GatewayTest()
        {
            this.StaticId = Guid.NewGuid();
        }

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

            TestsHelper.ValidateObject(obj);



            var mockGateway = new Mock<IGatewayService>();
            mockGateway.Setup(p => p.Add(obj)).Returns(true);

            var controller = new GatewayController(mockGateway.Object);

            var result = controller.Add(obj);

            Assert.True(result);
        }

        [Fact]
        public void Exception()
        {
            var obj = new ResReqGatewayVM
            {
                DateOfCreate = DateTime.Now,
                IPv4 = "Test",
                Name = "Test",
                SerialNumber = null
            };



            Assert.Throws<ValidationException>(() => TestsHelper.ValidateObject(obj));
        }

        [Fact]
        public void TestGetAllCount()
        {
            var list = new List<ResReqGatewayVM>();

            var obj = new ResReqGatewayVM
            {
                Id = Guid.NewGuid(),
                DateOfCreate = DateTime.Now,
                IPv4 = "Test",
                Name = "Test",
                SerialNumber = "Test"

            };

            list.Add(obj);

            var mockGateway = new Mock<IGatewayService>();
            mockGateway.Setup(p => p.GetAll()).Returns(list);

            var controller = new GatewayController(mockGateway.Object);

            var result = controller.GetAll();

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void TestGetAll()
        {
            var list = new List<ResReqGatewayVM>();

            var obj = new ResReqGatewayVM
            {
                Id = Guid.NewGuid(),
                DateOfCreate = DateTime.Now,
                IPv4 = "Test",
                Name = "Test",
                SerialNumber = "Test"

            };

            list.Add(obj);

            var mockGateway = new Mock<IGatewayService>();
            mockGateway.Setup(p => p.GetAll()).Returns(list);

            var controller = new GatewayController(mockGateway.Object);

            var result = controller.GetAll();

            Assert.Single(result);
        }

        [Fact]
        public void TestGetById()
        {
            var obj = new ResReqGatewayVM
            {
                Id = StaticId,
                DateOfCreate = DateTime.Now,
                IPv4 = "Test",
                Name = "Test",
                SerialNumber = "Test"
            };

            var mockGateway = new Mock<IGatewayService>();
            mockGateway.Setup(p => p.GetById(StaticId)).Returns(obj);

            var controller = new GatewayController(mockGateway.Object);

            var result = controller.GetById(StaticId);

            Assert.Equal(obj, result);
        }

    }
}
