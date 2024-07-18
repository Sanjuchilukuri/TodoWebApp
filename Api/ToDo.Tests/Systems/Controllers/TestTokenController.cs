using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using ToDo.Service.Interfaces;
using ToDo.Tests.MockData;
using ToDo.WebApi.Controllers;

namespace ToDo.Tests.Systems.Controllers
{
    public class TestTokenController
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IConfiguration> _config;

        public TestTokenController()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _config = new Mock<IConfiguration>();
        }

        [Fact]
        public void GetToken_ShouldReturn200Status_WhenTokenIsValid()
        {
            // Arrange
            var refreshToken = TokenMockData.validRefreshToken;
            _tokenServiceMock.Setup( _ => _.ValidateRefreshToken(refreshToken)).Returns(true);
            _tokenServiceMock.Setup(_ => _.GenerateNewTokens(refreshToken)).Returns(TokenMockData.GetTokens());
            var sut = new TokenController(_config.Object, _tokenServiceMock.Object);

            ///Act
            var result = sut.GetToken(refreshToken);

            ///Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult)?.StatusCode.Should().Be(200);
            (result as OkObjectResult)?.Value.Should().BeEquivalentTo(TokenMockData.GetTokens());
        }

        [Fact]
        public void GetToken_ShouldReturn401Status_WhenTokenIsInvalid()
        {
            ///Arrange
            var refreshToken = TokenMockData.inValidRefreshToken;
            _tokenServiceMock.Setup(_ => _.ValidateRefreshToken(refreshToken)).Returns(false);
            var sut = new TokenController(_config.Object, _tokenServiceMock.Object);

            ///Act
            var result = sut.GetToken(refreshToken);

            ///Assert
            result.GetType().Should().Be(typeof(UnauthorizedObjectResult));
            (result as UnauthorizedObjectResult)?.StatusCode.Should().Be(401);
            (result as UnauthorizedObjectResult)?.Value.Should().Be("Invalid token");
        }
    }
}
