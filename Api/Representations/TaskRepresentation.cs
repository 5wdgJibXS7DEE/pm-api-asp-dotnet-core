using System;
using ProjectManagement.Models;

namespace ProjectManagement.Api.Representations
{
    public class TaskRepresentation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime StartsOn { get; set; }

        public DateTime EndsOn { get; set; }

        public Guid AssigneeId { get; set; }

        public Guid ManagerId { get; set; }

        public TaskRepresentation(Task model)
        {
            Id = model.ExternalId;
            Name = model.Name;
            StartsOn = model.StartsOn;
            EndsOn = model.EndsOn;
            AssigneeId = model.AssigneeId;
            ManagerId = model.ManagerId;
        }
    }
}