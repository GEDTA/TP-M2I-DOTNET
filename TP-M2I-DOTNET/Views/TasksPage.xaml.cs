using System;
using Microsoft.Maui.Controls;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.ViewModels;

namespace TP_M2I_DOTNET.Views
{
    public partial class TasksPage : ContentPage
    {
        private TasksViewModel _viewModel;

        public TasksPage(TasksViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadTasksAsync();
        }

        private void OnStatusFilterChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                Models.TaskStatus? selectedStatus = picker.SelectedIndex switch
                {
                    0 => null, // Tous
                    1 => Models.TaskStatus.Todo, // À faire
                    2 => Models.TaskStatus.InProgress, // En cours
                    3 => Models.TaskStatus.Done, // Terminé
                    _ => null
                };

                _viewModel.FilterCommand.Execute(selectedStatus);
            }
        }
    }
}
