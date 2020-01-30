using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Representations;
using ProjectManagement.Definitions;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/task-overlaps")]
    public class TaskOverlapsController : Controller
    {
        private readonly ITasksLogic _tasks;

        private readonly ITaskOverlapsLogic _overlaps;

        public TaskOverlapsController(
            ITasksLogic taskLogic,
            ITaskOverlapsLogic taskOverlapsLogic)
        {
            _tasks = taskLogic;
            _overlaps = taskOverlapsLogic;
        }

        [HttpGet]
        public IEnumerable<TaskOverlapRepresentation> Get()
        {
            return _overlaps.All().Select(o => new TaskOverlapRepresentation(o, _tasks));
        }
    }
}