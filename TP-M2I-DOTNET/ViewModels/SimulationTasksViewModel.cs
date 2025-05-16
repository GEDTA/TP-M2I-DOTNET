using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.Services;

namespace TP_M2I_DOTNET.ViewModels
{
    public class SimulationTasksViewModel : INotifyPropertyChanged
    {
        private readonly ITaskService _taskService;
        private bool _isRefreshing;
        private ObservableCollection<TodoTask> _tasks;
        private List<TodoTask> _allTasks;
        private Models.TaskStatus? _currentFilter;

        public SimulationTasksViewModel(ITaskService simulationTaskService)
        {
            _taskService = simulationTaskService;
            Tasks = new ObservableCollection<TodoTask>();
            _allTasks = new List<TodoTask>();
            LoadTasksCommand = new Command(async () => await LoadTasksAsync());
            RefreshCommand = new Command(async () => await RefreshAsync());
            NavigateToApiCommand = new Command(async () => await NavigateToApi());
            FilterCommand = new Command<Models.TaskStatus?>(async (status) => await FilterTasksAsync(status));
            ViewTaskDetailsCommand = new Command<TodoTask>(async (task) => await ViewTaskDetailsAsync(task));
            AddNewTaskCommand = new Command(async () => await AddNewTaskAsync());
            
            // Charger les tâches initiales
            MainThread.BeginInvokeOnMainThread(async () => await LoadTasksAsync());
        }

        public ObservableCollection<TodoTask> Tasks
        {
            get => _tasks;
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadTasksCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NavigateToApiCommand { get; }
        public ICommand ViewTaskDetailsCommand { get; }
        public ICommand AddNewTaskCommand { get; }

        public async Task LoadTasksAsync()
        {
            try
            {
                IsRefreshing = true;
                var tasks = await _taskService.GetTasksAsync();
                _allTasks = tasks.ToList();
                
                Tasks.Clear();
                var filteredTasks = _currentFilter.HasValue
                    ? _allTasks.Where(t => t.Status == _currentFilter.Value)
                    : _allTasks;

                foreach (var task in filteredTasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                // Gérer l'erreur
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public async Task FilterTasksAsync(Models.TaskStatus? status)
        {
            try
            {
                IsRefreshing = true;
                _currentFilter = status;

                Tasks.Clear();
                var filteredTasks = status.HasValue
                    ? _allTasks.Where(t => t.Status == status.Value)
                    : _allTasks;

                foreach (var task in filteredTasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task RefreshAsync()
        {
            await LoadTasksAsync();
        }

        private async Task NavigateToApi()
        {
            await Shell.Current.GoToAsync("//tasks");
        }

        private async Task ViewTaskDetailsAsync(TodoTask task)
        {
            if (task == null)
                return;

            await Shell.Current.GoToAsync($"taskdetail?id={task.Id}&new=false&simulation=true");
        }

        private async Task AddNewTaskAsync()
        {
            await Shell.Current.GoToAsync("taskdetail?new=true&simulation=true");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
