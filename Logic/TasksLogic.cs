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

        public TaskOverlap Insert(Task task)
        {
            // todo GSA implement TasksLogic.Insert
            throw new NotImplementedException();
        }
    }
}