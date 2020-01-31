using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using ProjectManagement.Models;
using Xunit;
using ThreadingTask = System.Threading.Tasks.Task;

namespace ProjectManagement.Api.IntegrationTests.Tasks
{
    public class PostTests : IClassFixture<IntegrationFixture>
    {
        private readonly string _tasksEndpoint = "/api/tasks";

        private readonly IntegrationFixture _fixture;

        public PostTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async ThreadingTask Post_a_task_overlapping_three_others_but_only_two_have_the_same_assignee()
        {
            var assigneeIdOne = Guid.NewGuid();
            var assigneeIdTwo = Guid.NewGuid();

            // todo GSA code helper methods to help with JSON date with ISO format
            HttpContent postFebruaryTaskForAssigneeOne = CreateRequestToPostMonthlyTask(
                assigneeIdOne, "2020-02-01T00:00:00Z", "2020-03-01T00:00:00Z");
            HttpContent postMarchTaskForAssigneeOne = CreateRequestToPostMonthlyTask(
                assigneeIdOne, "2020-03-01T00:00:00Z", "2020-04-01T00:00:00Z");
            HttpContent postMarchTaskForAssigneeTwo = CreateRequestToPostMonthlyTask(
                assigneeIdTwo, "2020-03-01T00:00:00Z", "2020-04-01T00:00:00Z");
            HttpContent postOverlapingTaskForAssigneeOne = CreateRequestToPostMonthlyTask(
                assigneeIdOne, "2020-02-01T00:00:00Z", "2020-04-01T00:00:00Z");

            HttpResponseMessage response = await _fixture.Client.PostAsync(
                _tasksEndpoint, postFebruaryTaskForAssigneeOne);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            response = await _fixture.Client.PostAsync(
                _tasksEndpoint, postMarchTaskForAssigneeOne);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            response = await _fixture.Client.PostAsync(
                _tasksEndpoint, postMarchTaskForAssigneeTwo);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            response = await _fixture.Client.PostAsync(
                _tasksEndpoint, postOverlapingTaskForAssigneeOne);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            int tasksInStoreCount = _fixture.Store.GetCollection<Task>()
                .AsQueryable()
                .Count();
            Assert.Equal(4, tasksInStoreCount);

            int overlapsInStoreCount = _fixture.Store.GetCollection<TaskOverlap>()
                .AsQueryable()
                .Count();
            Assert.Equal(2, overlapsInStoreCount);

            // todo GSA how can I "continue" this test from another test file?
            // I would like to test GET /api/tasks in a Tasks/GetTests
            // then I would like to test GET /api/task-overlaps in a TaskOverlaps/GetTests
            // it would be nice to organize the test files in sync. with the Api controllers
        }

        private HttpContent CreateRequestToPostMonthlyTask(
            Guid assigneeId,
            string startDate,
            string endDate)
        {
            string anyName = "any name";
            string anyManagerId = "3d50b2d3-beef-47e2-bf0d-c441c14905d2";

            return new StringContent(
$@"{{
    ""name"": ""{anyName}"",
    ""startsOn"": ""{startDate}"",
    ""endsOn"": ""{endDate}"",
    ""assigneeId"": ""{assigneeId.ToString()}"",
    ""managerId"": ""{anyManagerId}""
}}",
                Encoding.UTF8,
                "application/json");
        }
    }
}