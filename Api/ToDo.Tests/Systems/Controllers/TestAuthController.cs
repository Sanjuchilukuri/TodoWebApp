using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using ToDo.Service.DTO;
using ToDo.Service.Interfaces;
using ToDo.Tests.MockData;
using ToDo.WebApi.Controllers;

namespace ToDo.Tests.Systems.Controllers
{
    public class TestAuthController
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<IConfiguration> _config;

        public TestAuthController()
        {
            _userService = new Mock<IUserService>();
            _tokenService = new Mock<ITokenService>();
            _config = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task Login_ShouldReturn200Status_WhenUserLoggedWithValidCredentials()
        {
            ///Arrange
            var user = UserMockData.GetValidUser();
            var validCredentials = UserMockData.ValidUserCredentials();

            _userService.Setup(_ => _.Login(validCredentials)).ReturnsAsync(user);
            _tokenService.Setup(_ => _.GenerateAccessToken(user.Id)).Returns("access-token");
            _tokenService.Setup(_ => _.GenerateRefreshToken(user.Id)).Returns("refresh-token");

            var sut = new AuthController(_userService.Object, _config.Object, _tokenService.Object);

            ///Act
            var result = await sut.Login(validCredentials);

            ///Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeEquivalentTo(new
            {
                jwttoken = "access-token",
                refreshToken = "refresh-token"
            });
        }

        [Fact]
        public async Task Login_ShouldReturn401Status_WhenUserLoggedWithInValidCredentials()
        {
            ///Arrange
            var inValidCredentials = UserMockData.InValidUserCredentials();
            _userService.Setup(_ => _.Login(inValidCredentials)).ReturnsAsync((UserDetails)null!);

            var sut = new AuthController(_userService.Object, _config.Object, _tokenService.Object);

            ///Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await sut.Login(inValidCredentials));
            exception.Message.Should().Be("Invalid credentials");
        }

        [Fact]
        public async Task Register_ShouldReturn201Status_WhenNewUserRegistered()
        {
            ///Arrange
            var newUserCredentials = UserMockData.NewuserCredentials();
            _userService.Setup(_ => _.Register(newUserCredentials)).ReturnsAsync(true);
            var sut = new AuthController(_userService.Object, _config.Object, _tokenService.Object);

            ///Act
            var result = await sut.Register(newUserCredentials);

            ///Assert
            result.GetType().Should().Be(typeof(CreatedResult));
            (result as CreatedResult)?.StatusCode.Should().Be(201);
            (result as CreatedResult)?.Value.Should().Be(UserMockData.NewuserCredentials().UserName);
        }

        [Fact]
        public async Task Register_ShouldReturn400Status_WhenUserAlreadyExists()
        {
            ///Arrange 
            var existedUserCredentials = UserMockData.ExisteduserCredentials();
            _userService.Setup(_ => _.IsUserExisted(existedUserCredentials.UserName)).ReturnsAsync(true);
            var sut = new AuthController(_userService.Object, _config.Object, _tokenService.Object);

            ///Act
            var result = await sut.Register(existedUserCredentials);

            ///Assert
            result.GetType().Should().Be(typeof(BadRequestObjectResult));
            (result as BadRequestObjectResult)?.StatusCode.Should().Be(400);
            (result as BadRequestObjectResult)?.Value.Should().Be("User already exists");
        }

        [Fact]
        public async Task Register_ShouldReturn500Status_WhenFailedToRegisterUser()
        {
            ///Arrange
            var newUserCredentials = UserMockData.NewuserCredentials();
            _userService.Setup(_ => _.Register(newUserCredentials)).ReturnsAsync(false);
            var sut = new AuthController(_userService.Object, _config.Object, _tokenService.Object);

            ///Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await sut.Register(newUserCredentials));
            exception.Message.Should().Be("Failed to Register User");
        }

    }
}
