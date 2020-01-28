using System.Collections.Generic;
using JsonFlatFileDataStore;
using ProjectManagement.Definitions;
using ProjectManagement.Models;

namespace ProjectManagement.Logic
{
    public class TaskOverlapsLogic : ITaskOverlapsLogic
    {
        private readonly IDataStore _store;

        public TaskOverlapsLogic(IDataStore dataStore)
        {
            _store = dataStore;
        }

        public IEnumerable<TaskOverlap> All()
        {
            return _store.GetCollection<TaskOverlap>().AsQueryable();
        }
    }
}