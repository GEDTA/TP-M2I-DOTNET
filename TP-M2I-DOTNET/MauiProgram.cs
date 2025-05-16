using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using TP_M2I_DOTNET.Services;
using TP_M2I_DOTNET.ViewModels;
using TP_M2I_DOTNET.Views;

namespace TP_M2I_DOTNET;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Configuration de l'API
		builder.Services.AddSingleton<ApiConfiguration>();

		// Configuration du HttpClient
		builder.Services.AddHttpClient("TasksApi", client =>
		{
			// L'URL de base sera configurée dans le service ApiTaskService
			client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));
		});

		// Enregistrer les services
		builder.Services.AddSingleton<ApiTaskService>();
		builder.Services.AddSingleton<SimulationTaskService>();
		builder.Services.AddSingleton<ITaskService>(sp => sp.GetRequiredService<ApiTaskService>());
		
		// Enregistrer les ViewModels
		builder.Services.AddTransient<TasksViewModel>();
		builder.Services.AddTransient<SimulationTasksViewModel>(sp =>
			new SimulationTasksViewModel(sp.GetRequiredService<SimulationTaskService>()));
		
		// Enregistrer les ViewModels pour les détails de tâche
		builder.Services.AddTransient<TaskDetailViewModel>(sp =>
			new TaskDetailViewModel(sp.GetRequiredService<ApiTaskService>()));
		
		// Créer une factory pour le ViewModel de simulation
		builder.Services.AddTransient<Func<SimulationTaskService, TaskDetailViewModel>>(sp =>
			simulationService => new TaskDetailViewModel(simulationService));
		
		// Enregistrer les pages
		builder.Services.AddTransient<TasksPage>();
		builder.Services.AddTransient<SimulationTasksPage>();
		builder.Services.AddTransient<TaskDetailPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
