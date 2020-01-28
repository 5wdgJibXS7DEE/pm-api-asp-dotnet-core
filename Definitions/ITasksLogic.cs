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
        /// A list with an <c>TaskOverlap</c> for each task that the inserted task overlaps.
        /// </returns>
        IEnumerable<TaskOverlap> Insert(Task insert);
    }
}