using CoindeskApi.Models.MetaData;
using CoindeskApi.Repository;
using CoindeskApiTest.MockData;
using Dapper;
using Moq;
using MyCommon.Interface;

namespace CoindeskApiTest.Repository
{
    public class CoindeskRepositoryTest
    {
        private readonly Mock<IMsDBConn> _mockDbConn;
        private readonly CoindeskRepository _repository;

        public CoindeskRepositoryTest()
        {
            _mockDbConn = new Mock<IMsDBConn>();
            _repository = new CoindeskRepository(_mockDbConn.Object);
        }
        [Fact]
        public async Task GetAll_Returns_CoindeskList()
        {
            // Arrange
            var expectedData =MockData_Coindesk.GetAll();

            _mockDbConn
                .Setup(conn => conn.QueryAsync<Coindesk>(It.IsAny<string>(), null))
                .ReturnsAsync(expectedData);

            // Act
            var result = await _repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("USD", result[0].Code);
        }

        [Fact]
        public async Task GetAssign_Returns_SingleCoindesk_WhenFound()
        {
            // Arrange
            var code = "USD";
            var expectedData = MockData_Coindesk.GetAssign(code);

            _mockDbConn
                .Setup(conn => conn.QueryAsync<Coindesk>(It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(new List<Coindesk> { expectedData });

            // Act
            var result = await _repository.GetAssign(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USD", result.Code);
            Assert.Equal("美元", result.CodeName);
        }

        [Fact]
        public void Add_Returns_True_WhenInserted()
        {
            // Arrange
            var expectedData = MockData_Coindesk.Add();

            _mockDbConn
                .Setup(conn => conn.Add(It.IsAny<Coindesk>(), It.IsAny<List<string>>()))
                .Returns(1); // 模拟插入成功返回 1

            // Act
            var result = _repository.Add(expectedData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Update_Returns_True_WhenUpdated()
        {
            // Arrange
            var coindesk = MockData_Coindesk.Update();

            _mockDbConn
                .Setup(conn => conn.Update<Coindesk>(It.IsAny<string[]>(), coindesk, It.IsAny<string[]>(), coindesk))
                .Returns(1); // 模拟更新成功返回 1

            // Act
            var result = _repository.Update(coindesk);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_Returns_True_WhenDeleted()
        {
            // Arrange
            var code = "USD";

            _mockDbConn
                .Setup(conn => conn.Excute(It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .Returns(1); // 模拟删除成功返回 1

            // Act
            var result = _repository.Delete(code);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsCheckHaveData_Returns_True_WhenDataExists()
        {
            // Arrange
            var code = "USD";
            var expectedData = MockData_Coindesk.GetAssign(code);

            _mockDbConn
                .Setup(conn => conn.QueryAsync<Coindesk>(It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(new List<Coindesk> { expectedData });

            // Act
            var result = await _repository.IsCheckHaveData(code);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsCheckHaveData_Returns_False_WhenNoData()
        {
            // Arrange
            var code = "USD";

            _mockDbConn
                .Setup(conn => conn.QueryAsync<Coindesk>(It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(new List<Coindesk>()); // 返回空数据

            // Act
            var result = await _repository.IsCheckHaveData(code);

            // Assert
            Assert.False(result);
        }
    }
}
