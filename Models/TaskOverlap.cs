namespace ProjectManagement.Models
{
    public class TaskOverlap : Entity
    {
        public int TaskOneId { get; set; }

        public int TaskTwoId { get; set; }
    }
}