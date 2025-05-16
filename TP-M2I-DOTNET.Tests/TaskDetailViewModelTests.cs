using System;
using System.Threading.Tasks;
using Moq;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.Services;
using TP_M2I_DOTNET.ViewModels;
using Xunit;

namespace TP_M2I_DOTNET.Tests
{
    public class TaskDetailViewModelTests
    {
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly TaskDetailViewModel _viewModel;
        private readonly TodoTask _testTask;

        public TaskDetailViewModelTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _testTask = new TodoTask
            {
                Id = 1,
                Title = "Test Task",
                Description = "Test Description",
                Status = Models.TaskStatus.Todo,
                Priority = TaskPriority.Medium,
                CreatedAt = DateTime.Now.AddDays(-1),
                UpdatedAt = DateTime.Now.AddDays(-1),
                DueDate = DateTime.Now.AddDays(1)
            };

            // Setup mock service
            _mockTaskService.Setup(s => s.GetTaskByIdAsync(1))
                .ReturnsAsync(_testTask);
            
            _mockTaskService.Setup(s => s.UpdateTaskAsync(It.IsAny<TodoTask>()))
                .ReturnsAsync((TodoTask task) => 
                {
                    task.UpdatedAt = DateTime.Now;
                    return task;
                });
                
            _mockTaskService.Setup(s => s.UpdateTaskStatusAsync(1, It.IsAny<Models.TaskStatus>()))
                .ReturnsAsync((int id, Models.TaskStatus status) => 
                {
                    _testTask.Status = status;
                    _testTask.UpdatedAt = DateTime.Now;
                    return _testTask;
                });
                
            _mockTaskService.Setup(s => s.AddTaskAsync(It.IsAny<TodoTask>()))
                .ReturnsAsync((TodoTask task) => 
                {
                    task.Id = 1;
                    task.CreatedAt = DateTime.Now;
                    task.UpdatedAt = DateTime.Now;
                    return task;
                });
                
            _mockTaskService.Setup(s => s.DeleteTaskAsync(1))
                .ReturnsAsync(true);

            _viewModel = new TaskDetailViewModel(_mockTaskService.Object);
        }

        [Fact]
        public async Task LoadTaskAsync_LoadsTaskById()
        {
            // Act
            await _viewModel.LoadTaskAsync(1);

            // Assert
            Assert.NotNull(_viewModel.Task);
            Assert.Equal(_testTask.Id, _viewModel.Task.Id);
            Assert.Equal(_testTask.Title, _viewModel.Task.Title);
            Assert.Equal(_testTask.Description, _viewModel.Task.Description);
            Assert.Equal(_testTask.Status, _viewModel.Task.Status);
            Assert.False(_viewModel.IsNew);
            _mockTaskService.Verify(s => s.GetTaskByIdAsync(1), Times.Once);
        }

        [Fact]
        public void CreateNewTask_InitializesNewTask()
        {
            // Act
            _viewModel.CreateNewTask();

            // Assert
            Assert.NotNull(_viewModel.Task);
            Assert.Equal(0, _viewModel.Task.Id);
            Assert.Equal(Models.TaskStatus.Todo, _viewModel.Task.Status);
            Assert.True(_viewModel.IsNew);
        }

        [Fact]
        public async Task SaveTaskAsync_ForNewTask_CallsAddTaskAsync()
        {
            // Arrange
            _viewModel.CreateNewTask();
            _viewModel.Task.Title = "New Task";
            _viewModel.Task.Description = "New Description";

            // Act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _mockTaskService.Verify(s => s.AddTaskAsync(It.IsAny<TodoTask>()), Times.Once);
            Assert.False(_viewModel.IsNew);
        }

        [Fact]
        public async Task SaveTaskAsync_ForExistingTask_CallsUpdateTaskAsync()
        {
            // Arrange
            await _viewModel.LoadTaskAsync(1);
            _viewModel.Task.Title = "Updated Title";

            // Act
            await _viewModel.SaveCommand.ExecuteAsync(null);

            // Assert
            _mockTaskService.Verify(s => s.UpdateTaskAsync(It.IsAny<TodoTask>()), Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_UpdatesTaskStatus()
        {
            // Arrange
            await _viewModel.LoadTaskAsync(1);
            var newStatus = Models.TaskStatus.InProgress;

            // Act
            await _viewModel.ChangeStatusCommand.ExecuteAsync(newStatus);

            // Assert
            Assert.Equal(newStatus, _viewModel.Task.Status);
            _mockTaskService.Verify(s => s.UpdateTaskStatusAsync(1, newStatus), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_CallsDeleteTaskAsync()
        {
            // Arrange
            await _viewModel.LoadTaskAsync(1);

            // Act
            await _viewModel.DeleteCommand.ExecuteAsync(null);

            // Assert
            _mockTaskService.Verify(s => s.DeleteTaskAsync(1), Times.Once);
        }

        [Fact]
        public void IsLoading_PropertyChangedEventFired()
        {
            // Arrange
            bool eventFired = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_viewModel.IsLoading))
                {
                    eventFired = true;
                }
            };

            // Act
            _viewModel.IsLoading = true;

            // Assert
            Assert.True(eventFired);
            Assert.True(_viewModel.IsLoading);
        }
    }
}
