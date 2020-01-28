using System;
using System.Collections.Generic;
using ProjectManagement.Models;

namespace ProjectManagement.Definitions
{
    public interface ITasksLogic
    {
        IEnumerable<Task> All();

        Task SingleById(int id);

        Task SingleOrDefaultByExternalId(Guid externalId);

        /// <returns>
        /// A <c>TaskOverlap</c> object if the assignee has a task overlapping the inserted task,
        /// <c>null</c> otherwise.
        /// </returns>
        TaskOverlap Insert(Task task);
    }
}