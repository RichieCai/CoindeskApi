using CoindeskApi.Controllers;
using CoindeskApi.Input;
using CoindeskApi.Interface.Service;
using CoindeskApi.Models.MetaData;
using CoindeskApi.ViewModels;
using CoindeskApiTest.MockData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;

namespace CoindeskApiTest.Controller
{
    public class CoindeskControllerTest
    {
        private readonly Mock<ICoindeskService> _mockService;
        private readonly Mock<IStringLocalizer<CoindeskController>> _mocklocalizer;
        private readonly CoindeskController _controller;
        public CoindeskControllerTest()
        {
            _mockService = new Mock<ICoindeskService>();
            _controller = new CoindeskController(_mockService.Object, _mocklocalizer.Object);
        }
        [Fact]
        public async Task CallApi_ReturnOk()
        {
            string sUrl = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var expectedResult = new ResultVM<Coindesk>
            {
                Success = true,
                Message = "資料已取得且新增成功",
                Data = new List<Coindesk>
                {
                    new Coindesk { Code = "BTC", CodeName = "Bitcoin" }
                }
            };
            _mockService.Setup(ser => ser.CallApi(sUrl)).ReturnsAsync(expectedResult);

            var vResult = await _controller.CallApi();

            var okResult= Assert.IsType<OkObjectResult>(vResult);
            Assert.Equal(expectedResult, okResult.Value);

        }
        [Fact]
        public async Task GetAssign_ReturnsOkResult_WhenCodeExists()
        {
            string scode = "USD";
            var expectedResult = MockData_Coindesk.GetAssign(scode);
            _mockService.Setup(s => s.GetAssign(scode)).ReturnsAsync(expectedResult);

            var vResult = await _controller.GetAssign(scode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(vResult);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithData()
        {
            // Arrange
            var expectedData = MockData_Coindesk.GetAll();
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedData, okResult.Value);
        }

        [Fact]
        public void Add_ReturnsOkResult_WhenModelIsValid()
        {
            // Arrange
            var expectedData = MockData_Coindesk.Add();
            var input = new ConindeskInput { code = "BTC", codename = "Bitcoin", ratefloat = 30000.5M };
            var expectedResult = new ResultVM<Coindesk> { Success = true, Message = "新增成功" };
            _mockService.Setup(s => s.Add(input)).Returns(expectedResult);

            // Act
            var result = _controller.Add(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public void Add_ReturnsBadRequest_WhenCodeIsNull()
        {
            // Arrange
            var input = new ConindeskInput { code = null, codename = "Bitcoin", ratefloat = 30000.5M };

            // Act
            var result = _controller.Add(input);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var actualResult = Assert.IsType<ResultVM<Coindesk>>(badRequestResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("請輸入幣別", actualResult.Message);
        }

        [Fact]
        public void Add_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var input = new ConindeskInput { code = "BTC", codename = null, ratefloat = 30000.5M };
            _controller.ModelState.AddModelError("codename", "Codename is required");

            // Act
            var result = _controller.Add(input);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var actualResult = Assert.IsType<ResultVM<Coindesk>>(badRequestResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("Validation failed", actualResult.Message);
            Assert.Contains("Codename is required", actualResult.Errors);
        }

        [Fact]
        public void Update_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var input = new ConindeskInput { code = "BTC", codename = "Bitcoin", ratefloat = 30000.5M };
            var expectedResult = new ResultVM<Coindesk> { Success = true, Message = "更新成功" };
            _mockService.Setup(s => s.Update(input)).Returns(expectedResult);

            // Act
            var result = _controller.Update(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public void Delete_ReturnsOkResult_WhenDeletionIsSuccessful()
        {
            // Arrange
            string code = "BTC";
            var expectedResult = new ResultVM<Coindesk> { Success = true, Message = "更新成功" };
            _mockService.Setup(s => s.Delete(code)).Returns(expectedResult);

            // Act
            var result = _controller.Delete(code);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }
    }
}
