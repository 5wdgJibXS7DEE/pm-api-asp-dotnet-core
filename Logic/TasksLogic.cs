using System;
using System.Collections.Generic;
using System.Linq;
using JsonFlatFileDataStore;
using ProjectManagement.Definitions;
using ProjectManagement.Models;

namespace ProjectManagement.Logic
{
    public class TasksLogic : ITasksLogic
    {
        private readonly IDataStore _store;

        public TasksLogic(IDataStore dataStore)
        {
            _store = dataStore;
        }

        public IEnumerable<Task> All()
        {
            return _store.GetCollection<Task>().AsQueryable();
        }

        public Task SingleById(int id)
        {
            return All().Single(t => t.Id == id);
        }

        public Task SingleOrDefaultByExternalId(Guid externalId)
        {
            return All().SingleOrDefault(t => t.ExternalId == externalId);
        }

        /// <remarks>
        /// Ensure you've generated a new Guid for <c>insert.ExternalId</c>, because
        /// this method does not check if it's already stored. This can result in
        /// duplicated keys.
        /// </remarks>
        public IEnumerable<TaskOverlap> Insert(Task insert)
        {
            IEnumerable<Task> overlapping = All()
                .Where(t => insert.AssigneeId == t.AssigneeId
                    && (insert.StartsOn < t.EndsOn && insert.EndsOn > t.StartsOn));

            if (_store.GetCollection<Task>().InsertOne(insert) == false)
                throw new ApplicationException("Inserting one task to the store");

            IEnumerable<TaskOverlap> overlaps = overlapping.Select(overlap => new TaskOverlap()
            {
                ExternalId = Guid.NewGuid(),
                TaskOneId = insert.Id,
                TaskTwoId = overlap.Id,
            });

            if (overlaps.Any())
                if (_store.GetCollection<TaskOverlap>().InsertMany(overlaps) == false)
                    throw new ApplicationException("Inserting many task overlaps to the store");

            return overlaps;
        }
    }
}