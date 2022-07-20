using CloudCustomers.API.Controllers;
using CloudCustomers.API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CloudCustomers.UnitTests.Systems.Controllers;

public class TestUsersController
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
    {
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Name = "Name",
                    Email = "email@email.com",
                    Address = new Address()
                    {
                        Street = "Street",
                        City = "City",
                        ZipCode = "12345"
                    }
                }
            });

        var sut = new UsersController(mockUsersService.Object);

        var result = (OkObjectResult)await sut.Get();

        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesUserServiceExactlyOnce()
    {
        var mockUsersService = new Mock<IUsersService>();

        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(mockUsersService.Object);

        var result = await sut.Get();

        mockUsersService.Verify(
            service => service.GetAllUsers(), Times.Once());
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUsers()
    {
        var mockUsersService = new Mock<IUsersService>();

        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Name = "Name",
                    Email = "email@email.com",
                    Address = new Address()
                    {
                        Street = "Street",
                        City = "City",
                        ZipCode = "12345"
                    }
                }
            });

        var sut = new UsersController(mockUsersService.Object);

        var result = await sut.Get();

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }

    [Fact]
    public async Task Get_OnNoUsersFound_ReturnsStatusCode404()
    {
        var mockUsersService = new Mock<IUsersService>();

        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UsersController(mockUsersService.Object);

        var result = await sut.Get();

        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);

    }


}