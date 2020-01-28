using System;
using ProjectManagement.Definitions;
using ProjectManagement.Models;

namespace ProjectManagement.Api.Representations
{
    public class TaskOverlapRepresentation
    {
        public Guid Id { get; set; }

        public TaskRepresentation TaskOne { get; set; }

        public TaskRepresentation TaskTwo { get; set; }

        public TaskOverlapRepresentation(
            TaskOverlap model,
            ITasksLogic tasks)
        {
            Id = model.ExternalId;
            TaskOne = new TaskRepresentation(tasks.SingleById(model.TaskOneId));
            TaskTwo = new TaskRepresentation(tasks.SingleById(model.TaskTwoId));
        }
    }
}