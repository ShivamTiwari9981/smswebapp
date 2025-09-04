using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagement.Controllers;
using StudentManagement.Models;

namespace StudentManagement.Tests;

public class StudentControllerTests
{
    private readonly Mock<ILogger<StudentController>> _mockLogger;
    private readonly StudentController _controller;

    public StudentControllerTests()
    {
        _mockLogger = new Mock<ILogger<StudentController>>();
        _controller = new StudentController(_mockLogger.Object);
    }
    [Fact]
    public void Index_ReturnsViewResult_WithStudents()
    {
        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<StudentViewModel>>(viewResult.Model);
        Assert.NotEmpty(model); // Should contain Alice Johnson from static list
    }
    [Fact]
    public void Create_PostValidModel_AddsStudent_AndRedirects()
    {
        // Arrange
        var newStudent = new StudentViewModel
        {
            FullName = "Bob Smith",
            Email = "bob@gmail.com"
        };

        // Act
        var result = _controller.Create(newStudent);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);

        // Verify student was added
        var students = _controller.Index() as ViewResult;
        var model = Assert.IsAssignableFrom<IEnumerable<StudentViewModel>>(students.Model);
        Assert.Contains(model, s => s.FullName == "Bob Smith" && s.Email == "bob@gmail.com");

        // Verify logger was called
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Bob Smith")),
                It.IsAny<System.Exception>(),
                (Func<It.IsAnyType, System.Exception, string>)It.IsAny<object>()),
            Times.Once);
    }


    [Fact]
    public void Create_PostInvalidModel_ReturnsRedirectToIndex()
    {
        // Arrange
        var invalidStudent = new StudentViewModel(); // FullName & Email missing
        _controller.ModelState.AddModelError("FullName", "Required");

        // Act
        var result = _controller.Create(invalidStudent);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);

        // Verify warning log
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Invalid model state")),
                It.IsAny<System.Exception>(),
                (Func<It.IsAnyType, System.Exception, string>)It.IsAny<object>()),
            Times.Once);
    }
}
