using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_M2I_DOTNET.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public enum TaskStatus
    {
        Todo,
        InProgress,
        Done
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }
}
