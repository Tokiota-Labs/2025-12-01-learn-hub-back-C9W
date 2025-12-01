using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LearnHub.Back.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using LearnHub.Back.Api;

namespace LearnHub.Back.Tests.Integration
{
    [TestFixture]
    public class CourseControllerHttpIntegrationTests
    {
        private WebApplicationFactory<Program> h_factory;
        private HttpClient h_client;

        [OneTimeSetUp]
        public void OneTimeSetup() {
            h_factory = new WebApplicationFactory<Program>();
            h_client = h_factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() {
            h_client.Dispose();
            h_factory.Dispose();
        }

        [Test]
        public async Task GetTopByDemand_ReturnsOkAndList() {
            // Act
            HttpResponseMessage h_response = await h_client.GetAsync("api/course/top-demand");

            // Assert
            h_response.StatusCode.Should().Be(HttpStatusCode.OK);
            List<CourseDto> lstCourseDto = await h_response.Content.ReadFromJsonAsync<List<CourseDto>>();
            lstCourseDto.Should().NotBeNull();
            lstCourseDto.Should().BeOfType<List<CourseDto>>();
        }
    }
}