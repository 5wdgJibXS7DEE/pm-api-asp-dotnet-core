using System;

namespace ProjectManagement.Models
{
    public class Task : Entity
    {
        public string Name { get; set; }

        public DateTime StartsOn { get; set; }

        public DateTime EndsOn { get; set; }

        public Guid AssigneeId { get; set; }

        public Guid ManagerId { get; set; }
    }
}