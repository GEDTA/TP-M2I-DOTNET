using System;
using System.Linq;
using System.Threading.Tasks;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.Services;
using Xunit;

namespace TP_M2I_DOTNET.Tests
{
    public class SimulationTaskServiceTests
    {
        private readonly SimulationTaskService _service;

        public SimulationTaskServiceTests()
        {
            _service = new SimulationTaskService();
        }

        [Fact]
        public async Task GetTasksAsync_ReturnsAllTasks()
        {
            // Act
            var tasks = await _service.GetTasksAsync();

            // Assert
            Assert.NotNull(tasks);
            Assert.True(tasks.Any());
        }

        [Theory]
        [InlineData(Models.TaskStatus.Todo)]
        [InlineData(Models.TaskStatus.InProgress)]
        [InlineData(Models.TaskStatus.Done)]
        public async Task GetTasksAsync_WithStatus_ReturnsFilteredTasks(Models.TaskStatus status)
        {
            // Act
            var tasks = await _service.GetTasksAsync(status);

            // Assert
            Assert.NotNull(tasks);
            Assert.All(tasks, task => Assert.Equal(status, task.Status));
        }

        [Fact]
        public async Task AddTaskAsync_AddsNewTask()
        {
            // Arrange
            var newTask = new TodoTask
            {
                Title = "Test Task",
                Description = "Test Description",
                Status = Models.TaskStatus.Todo,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.Now.AddDays(1)
            };

            // Act
            var result = await _service.AddTaskAsync(newTask);
            var allTasks = await _service.GetTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Contains(allTasks, t => t.Id == result.Id);
            Assert.Equal(newTask.Title, result.Title);
            Assert.Equal(newTask.Description, result.Description);
            Assert.Equal(newTask.Status, result.Status);
            Assert.Equal(newTask.Priority, result.Priority);
        }

        [Fact]
        public async Task UpdateTaskAsync_UpdatesExistingTask()
        {
            // Arrange
            var tasks = await _service.GetTasksAsync();
            var taskToUpdate = tasks.First();
            var updatedTitle = "Updated Title";
            taskToUpdate.Title = updatedTitle;

            // Act
            var result = await _service.UpdateTaskAsync(taskToUpdate);
            var updatedTask = await _service.GetTaskByIdAsync(taskToUpdate.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedTitle, result.Title);
            Assert.Equal(updatedTitle, updatedTask.Title);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_UpdatesTaskStatus()
        {
            // Arrange
            var tasks = await _service.GetTasksAsync();
            var taskToUpdate = tasks.First();
            var newStatus = taskToUpdate.Status == Models.TaskStatus.Todo 
                ? Models.TaskStatus.InProgress 
                : Models.TaskStatus.Todo;

            // Act
            var result = await _service.UpdateTaskStatusAsync(taskToUpdate.Id, newStatus);
            var updatedTask = await _service.GetTaskByIdAsync(taskToUpdate.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newStatus, result.Status);
            Assert.Equal(newStatus, updatedTask.Status);
        }

        [Fact]
        public async Task DeleteTaskAsync_RemovesTask()
        {
            // Arrange
            var tasks = await _service.GetTasksAsync();
            var taskToDelete = tasks.First();

            // Act
            var result = await _service.DeleteTaskAsync(taskToDelete.Id);
            var allTasksAfterDelete = await _service.GetTasksAsync();
            var deletedTask = await _service.GetTaskByIdAsync(taskToDelete.Id);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(allTasksAfterDelete, t => t.Id == taskToDelete.Id);
            Assert.Null(deletedTask);
        }
    }
}
