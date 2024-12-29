using CoindeskApi.Models.MetaData;
using CoindeskApi.Repository;
using Dapper;
using Moq;
using MyCommon.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoindeskApiTest.Repository
{
    public class CoindeskTWRepositroyTest
    {
        private readonly Mock<IMsDBConn> _mockDbConn;
        private readonly CoindeskTWRepositroy _repository;

        public CoindeskTWRepositroyTest()
        {
            _mockDbConn = new Mock<IMsDBConn>();
            _repository = new CoindeskTWRepositroy(_mockDbConn.Object);
        }

        [Fact]
        public async Task GetAll_Returns_CoindeskTwList()
        {
            // Arrange
            var expectedData = new List<CoindeskTw>
            {
                new CoindeskTw { Code = "BTC", CodeName = "Bitcoin" },
                new CoindeskTw { Code = "ETH", CodeName = "Ethereum" }
            };

            _mockDbConn
                .Setup(conn => conn.QueryAsync<CoindeskTw>(It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(expectedData);

            // Act
            var result = await _repository.GetAll();

            // Assert
            Assert.NotNull(result); // 确保返回值不为空
            Assert.Equal(2, result.Count); // 确保返回的数据项数正确
            Assert.Equal("BTC", result[0].Code); // 检查第一项的 Code
        }

        [Fact]
        public async Task GetAll_Returns_EmptyList_WhenNoData()
        {
            // Arrange
            _mockDbConn
                .Setup(conn => conn.QueryAsync<CoindeskTw>(It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(new List<CoindeskTw>()); // 模拟空数据返回

            // Act
            var result = await _repository.GetAll();

            // Assert
            Assert.NotNull(result); // 确保返回值不为空
            Assert.Empty(result); // 确保返回值为空列表
        }
    }
}
