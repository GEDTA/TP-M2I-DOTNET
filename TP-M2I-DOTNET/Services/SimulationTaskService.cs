using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP_M2I_DOTNET.Models;

namespace TP_M2I_DOTNET.Services
{
    public class SimulationTaskService : ITaskService
    {
        private List<TodoTask> _tasks;

        public SimulationTaskService()
        {
            // Initialiser avec des données de simulation
            _tasks = new List<TodoTask>
            {
                new TodoTask
                {
                    Id = 1,
                    Title = "Implémenter l'authentification",
                    Description = "Ajouter l'authentification JWT",
                    Status = Models.TaskStatus.InProgress,
                    Priority = TaskPriority.High,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    UpdatedAt = DateTime.Now.AddDays(-2),
                    DueDate = DateTime.Now.AddDays(10)
                },
                new Models.TodoTask
                {
                    Id = 2,
                    Title = "Concevoir la base de données",
                    Description = "Créer le schéma de la base de données pour l'application",
                    Status = Models.TaskStatus.Done,
                    Priority = TaskPriority.High,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    UpdatedAt = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(-2)
                },
                new Models.TodoTask
                {
                    Id = 3,
                    Title = "Implémenter la page d'accueil",
                    Description = "Créer la page d'accueil de l'application",
                    Status =  Models.TaskStatus.Todo,
                    Priority = TaskPriority.Medium,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    UpdatedAt = DateTime.Now.AddDays(-3),
                    DueDate = DateTime.Now.AddDays(15)
                },
                new Models.TodoTask
                {
                    Id = 4,
                    Title = "Tester les fonctionnalités",
                    Description = "Écrire des tests unitaires et d'intégration",
                    Status =  Models.TaskStatus.Todo,
                    Priority = TaskPriority.Low,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    UpdatedAt = DateTime.Now.AddDays(-2),
                    DueDate = DateTime.Now.AddDays(20)
                }
            };
        }

        public async Task<TodoTask> AddTaskAsync(TodoTask task)
        {
            task.Id = _tasks.Max(t => t.Id) + 1;
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;
            _tasks.Add(task);
            return await Task.FromResult(task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<TodoTask> GetTaskByIdAsync(int id)
        {
            return await Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<TodoTask>> GetTasksAsync(Models.TaskStatus? status = null)
        {
            await Task.Delay(100); // Simuler un délai réseau
            return status.HasValue
                ? _tasks.Where(t => t.Status == status.Value)
                : _tasks;
        }

        public async Task<TodoTask> UpdateTaskAsync(TodoTask task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.Priority = task.Priority;
                existingTask.Status = task.Status;
                existingTask.DueDate = task.DueDate;
                existingTask.UpdatedAt = DateTime.Now;
            }
            return await Task.FromResult(existingTask);
        }

        public async Task<TodoTask> UpdateTaskStatusAsync(int id, Models.TaskStatus status)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.Status = status;
                task.UpdatedAt = DateTime.Now;
            }
            return await Task.FromResult(task);
        }
    }
}
