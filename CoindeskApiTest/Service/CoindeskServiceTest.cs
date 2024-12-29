using CoindeskApi.Input;
using CoindeskApi.Interface.Repository;
using CoindeskApi.Interface.Service;
using CoindeskApi.Models.MetaData;
using CoindeskApi.Service;
using CoindeskApiTest.MockData;
using Moq;
using MyCommon.Interface;

namespace CoindeskApiTest.Service
{
    public class CoindeskServiceTest
    {
        private readonly Mock<ICoindeskRepository> _mockRepository;
        private readonly Mock<ICoindeskTWRepositroy> _twMockRepository;
        private readonly Mock<IMsDBConn> _msDBConnMock;
        private readonly ICoindeskService _service;
        public CoindeskServiceTest()
        {
            _mockRepository = new Mock<ICoindeskRepository>();
            _twMockRepository = new Mock<ICoindeskTWRepositroy>();
            _msDBConnMock = new Mock<IMsDBConn>();
            _service = new CoindeskService(_msDBConnMock.Object, _mockRepository.Object, _twMockRepository.Object);
        }
        [Fact]
        public async Task CallApi_Returns_ResultVM_WithSuccess()
        {
            // Arrange
            string url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            string sCode = "USD";
            var source = MockData_CoindeskTW.GetAll();
            var isHaveData = Task.FromResult(true);

            _mockRepository.Setup(r => r.IsCheckHaveData("USD")).Returns(isHaveData);
            _mockRepository.Setup(r => r.Delete("USD")).Verifiable();
            _twMockRepository.Setup(r => r.GetAll()).ReturnsAsync(source);
            _mockRepository.Setup(r => r.Add(It.IsAny<Coindesk>())).Returns(true);

            var result = await _service.CallApi(url);
            // Assert
            Assert.True(result.Success);
            Assert.Equal("資料已取得且新增成功", result.Message);

        }
        [Fact]
        public async Task GetAssign_ReturnOneData_IsHaveData()
        {
            string sCode = "USD";
            var source = MockData_Coindesk.GetAssign(sCode);

            _mockRepository.Setup(m => m.GetAssign(sCode)).ReturnsAsync(source);

           var vactual = _service.GetAssign(sCode);
            Assert.NotNull(vactual);
            Assert.Equal(source.Code,  vactual.Result.Code);
            Assert.Equal(source.CodeName, vactual.Result.CodeName);

        }

        [Fact]
        public async Task GetAll_ReturnAll_IsHaveData()
        {
            var source = MockData_Coindesk.GetAll();
            _mockRepository.Setup(m => m.GetAll()).ReturnsAsync(source);

            var vactual = _service.GetAll();

            Assert.NotNull(vactual);
            Assert.Equal(source.Count, vactual.Result.Count);
        }

        [Fact]
        public void Add_ReturnsSuccess_WhenInserted()
        {
            // Arrange
            var input = new ConindeskInput { code = "BTC", codename = "Bitcoin", ratefloat = 30000.5M };
            _mockRepository.Setup(r => r.IsCheckHaveData(input.code.ToUpper().Trim())).ReturnsAsync(false);
            _mockRepository.Setup(r => r.Add(It.IsAny<Coindesk>())).Returns(true);

            // Act
            var result = _service.Add(input);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("新增成功", result.Message);
        }

        [Fact]
        public void Add_Returns_Failure_WhenAlreadyExists()
        {
            // Arrange
            var input = new ConindeskInput { code = "BTC", codename = "Bitcoin", ratefloat = 30000.5M };
            _mockRepository.Setup(r => r.IsCheckHaveData(input.code.ToUpper().Trim())).ReturnsAsync(true);

            // Act
            var result = _service.Add(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("資料已存在", result.Message);
        }

        [Fact]
        public void Update_Returns_Success_WhenUpdated()
        {
            // Arrange
            var input = new ConindeskInput { code = "BTC", codename = "Bitcoin", ratefloat = 30000.5M };
            _mockRepository.Setup(r => r.IsCheckHaveData(input.code.ToUpper().Trim())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.Update(It.IsAny<Coindesk>())).Returns(true);

            // Act
            var result = _service.Update(input);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("更新成功", result.Message);
        }

        [Fact]
        public void Delete_Returns_Success_WhenDeleted()
        {
            // Arrange
            string code = "BTC";
            _mockRepository.Setup(r => r.IsCheckHaveData(code.Trim().ToUpper())).ReturnsAsync(true);
            _mockRepository.Setup(r => r.Delete(code.Trim().ToUpper())).Returns(true);

            // Act
            var result = _service.Delete(code);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("更新成功", result.Message);
        }

        [Fact]
        public void Delete_Returns_Failure_WhenNotFound()
        {
            // Arrange
            string code = "BTC";
            _mockRepository.Setup(r => r.IsCheckHaveData(code.Trim().ToUpper())).ReturnsAsync(false);

            // Act
            var result = _service.Delete(code);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("資料不存在", result.Message);
        }
    }
}
