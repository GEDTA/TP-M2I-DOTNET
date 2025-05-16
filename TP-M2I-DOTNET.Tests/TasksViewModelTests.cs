using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.Services;
using TP_M2I_DOTNET.ViewModels;
using Xunit;

namespace TP_M2I_DOTNET.Tests
{
    public class TasksViewModelTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly TasksViewModel _viewModel;
        private readonly List<TodoTask> _testTasks;

        public TasksViewModelTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _testTasks = new List<TodoTask>
            {
                new TodoTask
                {
                    Id = 1,
                    Title = "Test Task 1",
                    Description = "Description 1",
                    Status = Models.TaskStatus.Todo,
                    Priority = TaskPriority.Low,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                    DueDate = DateTime.Now.AddDays(1)
                },
                new TodoTask
                {
                    Id = 2,
                    Title = "Test Task 2",
                    Description = "Description 2",
                    Status = Models.TaskStatus.InProgress,
                    Priority = TaskPriority.Medium,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    UpdatedAt = DateTime.Now.AddDays(-1),
                    DueDate = DateTime.Now.AddDays(2)
                },
                new TodoTask
                {
                    Id = 3,
                    Title = "Test Task 3",
                    Description = "Description 3",
                    Status = Models.TaskStatus.Done,
                    Priority = TaskPriority.High,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    UpdatedAt = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(-1)
                }
            };

            // Setup mock service
            _mockTaskService.Setup(s => s.GetTasksAsync(It.IsAny<Models.TaskStatus?>()))
                .ReturnsAsync((Models.TaskStatus? status) => status.HasValue 
                    ? _testTasks.Where(t => t.Status == status.Value).ToList() 
                    : _testTasks);

            _viewModel = new TasksViewModel(_mockTaskService.Object);
        }

        [Fact]
        public async Task LoadTasksAsync_LoadsAllTasks()
        {
            // Act
            await _viewModel.LoadTasksAsync();

            // Assert
            Assert.Equal(_testTasks.Count, _viewModel.Tasks.Count);
            _mockTaskService.Verify(s => s.GetTasksAsync(It.IsAny<Models.TaskStatus?>()), Times.Once);
        }

        [Theory]
        [InlineData(Models.TaskStatus.Todo, 1)]
        [InlineData(Models.TaskStatus.InProgress, 1)]
        [InlineData(Models.TaskStatus.Done, 1)]
        public async Task FilterTasksAsync_FiltersTasksByStatus(Models.TaskStatus status, int expectedCount)
        {
            // Act
            await _viewModel.FilterTasksAsync(status);

            // Assert
            Assert.Equal(expectedCount, _viewModel.Tasks.Count);
            Assert.All(_viewModel.Tasks, task => Assert.Equal(status, task.Status));
            _mockTaskService.Verify(s => s.GetTasksAsync(status), Times.Once);
        }

        [Fact]
        public void IsRefreshing_PropertyChangedEventFired()
        {
            // Arrange
            bool eventFired = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_viewModel.IsRefreshing))
                {
                    eventFired = true;
                }
            };

            // Act
            _viewModel.IsRefreshing = true;

            // Assert
            Assert.True(eventFired);
            Assert.True(_viewModel.IsRefreshing);
        }

        [Fact]
        public void Tasks_PropertyChangedEventFired()
        {
            // Arrange
            bool eventFired = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_viewModel.Tasks))
                {
                    eventFired = true;
                }
            };

            // Act
            _viewModel.Tasks = new System.Collections.ObjectModel.ObservableCollection<TodoTask>();

            // Assert
            Assert.True(eventFired);
        }
    }
}
