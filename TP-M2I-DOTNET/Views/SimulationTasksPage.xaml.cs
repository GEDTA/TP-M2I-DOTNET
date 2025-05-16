using System;
using Microsoft.Maui.Controls;
using TP_M2I_DOTNET.Models;
using TP_M2I_DOTNET.ViewModels;

namespace TP_M2I_DOTNET.Views
{
    public partial class SimulationTasksPage : ContentPage
    {
        private SimulationTasksViewModel _viewModel;

        public SimulationTasksPage(SimulationTasksViewModel viewModel)
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
        
        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Models.TodoTask selectedTask)
            {
                // Désélectionner l'élément pour permettre une nouvelle sélection
                ((CollectionView)sender).SelectedItem = null;
                
                // Naviguer vers la page de détail avec le paramètre simulation=true
                await Shell.Current.GoToAsync($"taskdetail?id={selectedTask.Id}&new=false&simulation=true");
            }
        }
    }
}
