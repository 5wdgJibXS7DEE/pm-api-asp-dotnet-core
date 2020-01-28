using System.Collections.Generic;
using ProjectManagement.Models;

namespace ProjectManagement.Definitions
{
    public interface ITaskOverlapsLogic
    {
        IEnumerable<TaskOverlap> All();
    }
}