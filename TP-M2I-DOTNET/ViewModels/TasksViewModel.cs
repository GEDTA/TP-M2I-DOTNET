using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.Services;

namespace TP_M2I_DOTNET.ViewModels
{
    public class TasksViewModel : INotifyPropertyChanged
    {
        private readonly ITaskService _taskService;
        private ObservableCollection<TodoTask> _tasks;
        private Models.TaskStatus? _selectedStatus;
        private bool _isRefreshing;
        private bool _isBusy;
        private string _title;

        public TasksViewModel(ITaskService taskService)
        {
            _title = "Tâches";
            _taskService = taskService;
            Tasks = new ObservableCollection<TodoTask>();
            LoadTasksCommand = new Command(async () => await LoadTasksAsync());
            FilterCommand = new Command<Models.TaskStatus?>(async (status) => await FilterTasksAsync(status));
            RefreshCommand = new Command(async () => await RefreshAsync());
            NavigateToSimulationCommand = new Command(async () => await NavigateToSimulation());
            NavigateToApiCommand = new Command(async () => await NavigateToApi());
            ViewTaskDetailsCommand = new Command<TodoTask>(async (task) => await ViewTaskDetailsAsync(task));
            AddNewTaskCommand = new Command(async () => await AddNewTaskAsync());
            
            // Charger les tâches initiales
            MainThread.BeginInvokeOnMainThread(async () => await LoadTasksAsync());
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
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

        public Models.TaskStatus? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                    FilterCommand.Execute(value);
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
        public ICommand FilterCommand { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand NavigateToSimulationCommand { get; }
        public ICommand NavigateToApiCommand { get; }
        public ICommand ViewTaskDetailsCommand { get; }
        public ICommand AddNewTaskCommand { get; }

        public async Task LoadTasksAsync()
        {
            if (_isBusy)
                return;

            IsBusy = true;

            try
            {
                Tasks.Clear();
                var tasks = await _taskService.GetTasksAsync(_selectedStatus);
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception (idéalement avec un service de logging)
                Console.WriteLine($"Erreur lors du chargement des tâches: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task FilterTasksAsync(Models.TaskStatus? status)
        {
            _selectedStatus = status;
            await LoadTasksAsync();
        }

        public async Task RefreshAsync()
        {
            IsRefreshing = true;
            await LoadTasksAsync();
            IsRefreshing = false;
        }

        private async Task NavigateToSimulation()
        {
            await Shell.Current.GoToAsync("//simulation");
        }

        private async Task NavigateToApi()
        {
            await Shell.Current.GoToAsync("//tasks");
        }

        private async Task ViewTaskDetailsAsync(TodoTask task)
        {
            if (task == null)
                return;

            await Shell.Current.GoToAsync($"taskdetail?id={task.Id}&new=false&simulation=false");
        }

        private async Task AddNewTaskAsync()
        {
            await Shell.Current.GoToAsync("taskdetail?new=true&simulation=false");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}