using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using TP_M2I_DOTNET.Services;
using TP_M2I_DOTNET.ViewModels;

namespace TP_M2I_DOTNET.Views
{
    [QueryProperty(nameof(TaskId), "id")]
    [QueryProperty(nameof(IsNew), "new")]
    [QueryProperty(nameof(IsSimulation), "simulation")]
    public partial class TaskDetailPage : ContentPage
    {
        private readonly TaskDetailViewModel _apiViewModel;
        private readonly Func<SimulationTaskService, TaskDetailViewModel> _simulationViewModelFactory;
        private readonly SimulationTaskService _simulationService;
        private TaskDetailViewModel _currentViewModel;
        private int _taskId;
        private bool _isNew;
        private bool _isSimulation;

        public TaskDetailPage(
            TaskDetailViewModel apiViewModel, 
            Func<SimulationTaskService, TaskDetailViewModel> simulationViewModelFactory,
            SimulationTaskService simulationService)
        {
            InitializeComponent();
            _apiViewModel = apiViewModel;
            _simulationViewModelFactory = simulationViewModelFactory;
            _simulationService = simulationService;
        }

        public string TaskId
        {
            set
            {
                _taskId = int.Parse(value);
            }
        }

        public string IsNew
        {
            set
            {
                _isNew = bool.Parse(value);
            }
        }

        public string IsSimulation
        {
            set
            {
                _isSimulation = bool.Parse(value);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Sélectionner le ViewModel approprié
            _currentViewModel = _isSimulation 
                ? _simulationViewModelFactory(_simulationService) 
                : _apiViewModel;
            BindingContext = _currentViewModel;
            
            // S'abonner à l'événement TaskUpdated
            _currentViewModel.TaskUpdated += OnTaskUpdated;

            if (_isNew)
            {
                _currentViewModel.CreateNewTask();
            }
            else
            {
                await _currentViewModel.LoadTaskAsync(_taskId);
            }
        }
        
        private void OnTaskUpdated(Models.TodoTask updatedTask)
        {
            // Rafraîchir la liste des tâches après une mise à jour
            // Cette méthode sera appelée juste avant de revenir à la page précédente
            if (_isSimulation)
            {
                // Rafraîchir la liste de simulation
                var simulationViewModel = Application.Current.MainPage.Handler.MauiContext.Services
                    .GetService<SimulationTasksViewModel>();
                if (simulationViewModel != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () => await simulationViewModel.LoadTasksAsync());
                }
            }
            else
            {
                // Rafraîchir la liste API
                var tasksViewModel = Application.Current.MainPage.Handler.MauiContext.Services
                    .GetService<TasksViewModel>();
                if (tasksViewModel != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () => await tasksViewModel.LoadTasksAsync());
                }
            }
        }
    }
}
