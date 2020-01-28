using System;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult All()
        {
            // todo GSA implement TasksController.All
            throw new NotImplementedException();
        }
    }
}