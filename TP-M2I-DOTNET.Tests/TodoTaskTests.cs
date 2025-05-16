using System;
using TP_M2I_DOTNET.Models;
using Xunit;

namespace TP_M2I_DOTNET.Tests
{
    public class TodoTaskTests
    {
        [Fact]
        public void TodoTask_PropertiesInitializeCorrectly()
        {
            // Arrange
            var now = DateTime.Now;
            var dueDate = now.AddDays(7);
            
            // Act
            var task = new TodoTask
            {
                Id = 1,
                Title = "Test Task",
                Description = "Test Description",
                Status = TaskStatus.Todo,
                Priority = TaskPriority.High,
                CreatedAt = now,
                UpdatedAt = now,
                DueDate = dueDate
            };
            
            // Assert
            Assert.Equal(1, task.Id);
            Assert.Equal("Test Task", task.Title);
            Assert.Equal("Test Description", task.Description);
            Assert.Equal(TaskStatus.Todo, task.Status);
            Assert.Equal(TaskPriority.High, task.Priority);
            Assert.Equal(now, task.CreatedAt);
            Assert.Equal(now, task.UpdatedAt);
            Assert.Equal(dueDate, task.DueDate);
        }
        
        [Fact]
        public void TodoTask_DefaultValues()
        {
            // Act
            var task = new TodoTask();
            
            // Assert
            Assert.Equal(0, task.Id);
            Assert.Null(task.Title);
            Assert.Null(task.Description);
            Assert.Equal(TaskStatus.Todo, task.Status); // Vérifie si la valeur par défaut est Todo
            Assert.Equal(TaskPriority.Medium, task.Priority); // Vérifie si la valeur par défaut est Medium
            Assert.Equal(default, task.CreatedAt);
            Assert.Equal(default, task.UpdatedAt);
            Assert.Equal(default, task.DueDate);
        }
    }
}
