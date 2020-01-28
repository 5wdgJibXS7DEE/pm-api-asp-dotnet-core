using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Representations;
using ProjectManagement.Definitions;
using ProjectManagement.Models;

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
        public IEnumerable<TaskRepresentation> Get()
        {
            return _tasks.All().Select(t => new TaskRepresentation(t));
        }

        [HttpGet("{id}")]
        public ActionResult<TaskRepresentation> Get(Guid id)
        {
            Task model = _tasks.SingleOrDefaultByExternalId(id);

            if (model == null)
                return NotFound();

            return new TaskRepresentation(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TaskRepresentation input)
        {
            if (ModelState.IsValid == false)
                // todo GSA return more readable validation errors
                return new BadRequestObjectResult(ModelState);

            Task model = input.ToModel();

            _tasks.Insert(model);

            return CreatedAtAction(nameof(Get), new { id = model.ExternalId }, new TaskRepresentation(model));
        }
    }
}