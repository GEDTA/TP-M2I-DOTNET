using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.Services;

namespace TP_M2I_DOTNET.ViewModels
{
    // Délégué pour l'événement de mise à jour d'une tâche
    public delegate void TaskUpdatedEventHandler(TodoTask updatedTask);
    
    public class TaskDetailViewModel : INotifyPropertyChanged
    {
        private readonly ITaskService _taskService;
        private TodoTask _task;
        private bool _isLoading;
        private bool _isNew;
        
        // Événement déclenché lorsqu'une tâche est mise à jour ou créée
        public event TaskUpdatedEventHandler TaskUpdated;

        public TaskDetailViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            SaveCommand = new Command(async () => await SaveTaskAsync());
            DeleteCommand = new Command(async () => await DeleteTaskAsync());
            ChangeStatusCommand = new Command<Models.TaskStatus>(async (status) => await ChangeStatusAsync(status));
        }

        public bool IsNew
        {
            get => _isNew;
            set
            {
                if (_isNew != value)
                {
                    _isNew = value;
                    OnPropertyChanged();
                }
            }
        }

        public TodoTask Task
        {
            get => _task;
            set
            {
                if (_task != value)
                {
                    _task = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ChangeStatusCommand { get; }

        public async Task LoadTaskAsync(int id)
        {
            try
            {
                IsLoading = true;
                Task = await _taskService.GetTaskByIdAsync(id);
                IsNew = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible de charger la tâche : {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void CreateNewTask()
        {
            Task = new TodoTask
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = Models.TaskStatus.Todo,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.Now.AddDays(7)
            };
            IsNew = true;
        }

        private async Task SaveTaskAsync()
        {
            try
            {
                IsLoading = true;
                
                if (IsNew)
                {
                    Task = await _taskService.AddTaskAsync(Task);
                    IsNew = false;
                }
                else
                {
                    Task = await _taskService.UpdateTaskAsync(Task);
                }
                
                // Déclencher l'événement de mise à jour
                TaskUpdated?.Invoke(Task);
                
                await Shell.Current.DisplayAlert("Succès", "Tâche enregistrée avec succès", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible d'enregistrer la tâche : {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteTaskAsync()
        {
            try
            {
                if (IsNew)
                {
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                bool confirm = await Shell.Current.DisplayAlert("Confirmation", 
                    "Êtes-vous sûr de vouloir supprimer cette tâche ?", "Oui", "Non");
                
                if (!confirm)
                    return;

                IsLoading = true;
                bool success = await _taskService.DeleteTaskAsync(Task.Id);
                
                if (success)
                {
                    await Shell.Current.DisplayAlert("Succès", "Tâche supprimée avec succès", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible de supprimer la tâche", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible de supprimer la tâche : {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ChangeStatusAsync(Models.TaskStatus newStatus)
        {
            try
            {
                if (IsNew)
                {
                    Task.Status = newStatus;
                    OnPropertyChanged(nameof(Task));
                    return;
                }

                IsLoading = true;
                Task = await _taskService.UpdateTaskStatusAsync(Task.Id, newStatus);
                await Shell.Current.DisplayAlert("Succès", "Statut mis à jour avec succès", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible de mettre à jour le statut : {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
