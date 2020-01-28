using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Representations;
using ProjectManagement.Definitions;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/tasks")]
    public class TasksController : Controller
    {
        private readonly ITasksLogic _tasks;

        public TasksController(ITasksLogic tasksLogic)
        {
            _tasks = tasksLogic;
        }

        [HttpGet]
        public IEnumerable<TaskRepresentation> All()
        {
            return _tasks.All().Select(t => new TaskRepresentation(t));
        }
    }
}