using System;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Definitions;

namespace ProjectManagement.Api.Controllers
{
    [Route("api/task-overlaps")]
    public class TaskOverlapsController : Controller
    {
        private readonly ITaskOverlapsLogic _overlaps;

        public TaskOverlapsController(ITaskOverlapsLogic taskOverlapsLogic)
        {
            _overlaps = taskOverlapsLogic;
        }

        [HttpGet]
        public IActionResult All()
        {
            // todo GSA implement TaskOverlapsController.All
            throw new NotImplementedException();
        }
    }
}