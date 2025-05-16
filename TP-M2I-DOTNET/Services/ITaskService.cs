using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TP_M2I_DOTNET.Models;

namespace TP_M2I_DOTNET.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TodoTask>> GetTasksAsync(Models.TaskStatus? status = null);
        Task<TodoTask> GetTaskByIdAsync(int id);
        Task<TodoTask> AddTaskAsync(TodoTask task);
        Task<TodoTask> UpdateTaskAsync(TodoTask task);
        Task<bool> DeleteTaskAsync(int id);
        Task<TodoTask> UpdateTaskStatusAsync(int id, Models.TaskStatus status);
    }
}
