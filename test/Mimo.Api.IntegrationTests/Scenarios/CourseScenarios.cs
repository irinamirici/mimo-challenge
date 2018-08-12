using Mimo.Api.Commands;
using Mimo.Api.Dtos;
using Mimo.Api.IntegrationTests.Infrastructure;
using Mimo.Api.Messages;
using Mimo.Persistence.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mimo.Api.IntegrationTests.Scenarios
{
    public class CourseScenarios : AbstractScenario
    {
        private List<string> coursesToDelete = new List<string>();

        public CourseScenarios(TestServerFixture testServerFixture) : base(testServerFixture)
        { }

        [Fact]
        public async Task MimoUserCanRetrieveCourses()
        {
            var response = await httpClient.GetAsync("api/courses");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var course = JsonConvert.DeserializeObject<IEnumerable<CourseDto>>(await response.Content.ReadAsStringAsync());
            Assert.True(course.Count() >= 3);
        }

        [Fact]
        public async Task ContentCreatorUserCanRetrieveCourses()
        {
            var response = await contentCreatorHttpClient.GetAsync("api/courses");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var course = JsonConvert.DeserializeObject<IEnumerable<CourseDto>>(await response.Content.ReadAsStringAsync());
            Assert.True(course.Count() >= 3);
        }

        [Fact]
        public async Task MimoUserCannotCreateCourse()
        {
            var course = new CreateCourseCommand
            {
                Description = "test",
                Name = "create"
            };
            var response = await httpClient.PostAsync("api/courses", BuildJsonContent(course)                );

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ContentCreatorCanCreateCourse()
        {
            var name = Guid.NewGuid().ToString();
            var course = new CreateCourseCommand
            {
                Description = "test",
                Name = name
            };
            var response = await contentCreatorHttpClient.PostAsync("api/courses", BuildJsonContent(course));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = JsonConvert.DeserializeObject<CourseDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(course.Name, created.Name);
            Assert.Equal(course.Description, created.Description);
            Assert.True(created.Id > 0);
            Assert.False(created.IsPublished);
            coursesToDelete.Add(name);
        }

        [Fact]
        public async Task CreateCourseReturnsDuplicateError()
        {
            var name = Guid.NewGuid().ToString();
            dbContext.Courses.Add(new Course { Name = name, Description = "tests" });
            dbContext.SaveChanges();

            var course = new CreateCourseCommand
            {
                Description = "test",
                Name = name
            };
            var response = await contentCreatorHttpClient.PostAsync("api/courses", BuildJsonContent(course));

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            var errors = JsonConvert.DeserializeObject<IEnumerable<ResultError>>(await response.Content.ReadAsStringAsync());
            Assert.Single(errors);
            Assert.Equal(Constants.ErrorCodes.Duplicate, errors.First().ErrorCode);
            coursesToDelete.Add(name);
        }

        public override void Dispose()
        {
            dbContext.Courses.RemoveRange(dbContext.Courses.Where(x => coursesToDelete.Contains(x.Name)));
            dbContext.SaveChanges();
            base.Dispose();
        }
    }
}
