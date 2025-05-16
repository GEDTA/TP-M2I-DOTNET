using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TP_M2I_DOTNET.Models;

namespace TP_M2I_DOTNET.Services
{
    public class ApiTaskService : ITaskService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string CLIENT_NAME = "TasksApi";

        public ApiTaskService(ApiConfiguration config, IHttpClientFactory clientFactory)
        {
            _baseUrl = config.BaseUrl;
            _clientFactory = clientFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<TodoTask> AddTaskAsync(TodoTask task)
        {
            try
            {
                var client = _clientFactory.CreateClient(CLIENT_NAME);
                var response = await client.PostAsJsonAsync($"{_baseUrl}/api/tasks", task);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TodoTask>(_jsonOptions);
                }
                
                throw new ApiException(response.StatusCode, 
                    await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, 
                    "Une erreur s'est produite lors de l'ajout de la tâche : " + ex.Message);
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                var client = _clientFactory.CreateClient(CLIENT_NAME);
                var response = await client.DeleteAsync($"{_baseUrl}/api/tasks/{id}");
                
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return true;
                    
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return false;
                    
                throw new ApiException(response.StatusCode, 
                    await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, 
                    "Une erreur s'est produite lors de la suppression de la tâche : " + ex.Message);
            }
        }

        public async Task<TodoTask> GetTaskByIdAsync(int id)
        {
            try
            {
                var client = _clientFactory.CreateClient(CLIENT_NAME);
                var response = await client.GetAsync($"{_baseUrl}/api/tasks/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TodoTask>(_jsonOptions);
                }
                
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                    
                throw new ApiException(response.StatusCode, 
                    await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, 
                    "Une erreur s'est produite lors de la récupération de la tâche : " + ex.Message);
            }
        }

        public async Task<IEnumerable<TodoTask>> GetTasksAsync(Models.TaskStatus? status = null)
        {
            try
            {
                var client = _clientFactory.CreateClient(CLIENT_NAME);
                var url = $"{_baseUrl}/api/tasks";
                if (status.HasValue)
                {
                    url += $"?status={status.Value.ToString().ToLower()}";
                }

                var response = await client.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<TodoTask>>(_jsonOptions);
                }
                
                throw new ApiException(response.StatusCode, 
                    await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, 
                    "Une erreur s'est produite lors de la récupération des tâches : " + ex.Message);
            }
        }

        public async Task<TodoTask> UpdateTaskAsync(TodoTask task)
        {
            try
            {
                var client = _clientFactory.CreateClient(CLIENT_NAME);
                var response = await client.PutAsJsonAsync($"{_baseUrl}/api/tasks/{task.Id}", task);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TodoTask>(_jsonOptions);
                }
                
                throw new ApiException(response.StatusCode, 
                    await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, 
                    "Une erreur s'est produite lors de la mise à jour de la tâche : " + ex.Message);
            }
        }

        public async Task<TodoTask> UpdateTaskStatusAsync(int id, Models.TaskStatus status)
        {
            try
            {
                var client = _clientFactory.CreateClient(CLIENT_NAME);
                var statusUpdate = new { status = status.ToString().ToLower() };
                var response = await client.PutAsJsonAsync($"{_baseUrl}/api/tasks/{id}/status", statusUpdate);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TodoTask>(_jsonOptions);
                }
                
                throw new ApiException(response.StatusCode, 
                    await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, 
                    "Une erreur s'est produite lors de la mise à jour du statut de la tâche : " + ex.Message);
            }
        }
    }
}
