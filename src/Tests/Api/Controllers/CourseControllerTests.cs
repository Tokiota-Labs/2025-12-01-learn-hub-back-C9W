using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LearnHub.Back.Api.Controllers;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LearnHub.Back.Tests.Api.Controllers
{
    public class CourseControllerTests
    {
        [Fact]
        public async Task GetTopByDemand_ReturnsOkResult_WithListOfCourseDto()
        {
            // Arrange
            List<CourseDto> lstCourseDto = new List<CourseDto>
            {
                new CourseDto { Id = Guid.NewGuid(), Name = "Course1", Description = "Desc1", EnrollmentCount = 5 }
            };
            var moMediator = new Mock<IMediator>();
            moMediator.Setup(m => m.Send(It.IsAny<GetTopCoursesByDemandQuery>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(lstCourseDto);
            var ctrlCourse = new CourseController(moMediator.Object);

            // Act
            ActionResult<List<CourseDto>> actResult = await ctrlCourse.GetTopByDemand();

            // Assert
            Assert.IsType<OkObjectResult>(actResult.Result);
            var okResult = actResult.Result as OkObjectResult;
            var lstResult = Assert.IsType<List<CourseDto>>(okResult.Value);
            Assert.Equal(lstCourseDto, lstResult);
        }
    }
}