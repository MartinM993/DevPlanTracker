using System.Text.Json;
using Microsoft.JSInterop;
using DevPlanTracker.Models;

namespace DevPlanTracker.Services
{
    public class TaskStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string StorageKey = "devPlanTrackerDataV3";

        public TaskStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<List<DevTask>> LoadTasksAsync()
        {
            var savedJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(savedJson))
            {
                var tasks = JsonSerializer.Deserialize<List<DevTask>>(savedJson);
                if (tasks != null && tasks.Any()) return tasks;
            }
            return GetInitialTasks();
        }

        public async Task SaveTasksAsync(List<DevTask> tasks)
        {
            var json = JsonSerializer.Serialize(tasks);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }

        public async Task ExportTasksAsync(List<DevTask> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            await _jsRuntime.InvokeVoidAsync("downloadFile", $"DevTracker_Backup_{DateTime.Now:yyyy-MM-dd}.json", json);
        }

        public async Task<List<DevTask>?> ImportTasksAsync(Stream stream)
        {
            using var reader = new System.IO.StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<List<DevTask>>(json);
        }

        public async Task<string> GetEditorHtmlAsync(int taskId)
        {
            return await _jsRuntime.InvokeAsync<string>("getEditorHtml", $"editor-{taskId}");
        }

        public async Task SetEditorHtmlAsync(int taskId, string html)
        {
            await _jsRuntime.InvokeVoidAsync("setEditorHtml", $"editor-{taskId}", html);
        }

        public async Task ShowAlertAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("alert", message);
        }

        private List<DevTask> GetInitialTasks()
        {
            return new List<DevTask>
            {
                new DevTask { Id = 1, Area = "Delivery Excellence", Type = "Course/Training", Description = "Complete a course on CI/CD pipelines.", Goal = "Understand core concepts." },
                new DevTask { Id = 2, Area = "Delivery Excellence", Type = "Shadowing", Description = "Pair with a senior engineer to observe CI/CD management.", Goal = "Manage pipelines with minimal guidance." },
                new DevTask { Id = 3, Area = "Delivery Excellence", Type = "New Responsibility", Description = "Effectively use CI/CD pipelines in daily workflow.", Goal = "Consistent use of pipelines." },
                new DevTask { Id = 4, Area = "Delivery Excellence", Type = "Stretch Assignment", Description = "Troubleshoot build/deployment processes and resolve pipeline failures.", Goal = "Manage independently." },
                new DevTask { Id = 5, Area = "Delivery Excellence", Type = "Stretch Assignment", Description = "Establish quality gates with code coverage thresholds.", Goal = "Enforce thresholds and improve code quality." },
                new DevTask { Id = 6, Area = "Continuous Delivery", Type = "Course/Training", Description = "Complete a course on GitHub Actions and CI/CD Management.", Goal = "Explain how pipelines operate." },
                new DevTask { Id = 7, Area = "Continuous Delivery", Type = "Shadowing", Description = "Shadow a senior DevOps engineer during pipeline management.", Goal = "Participate in guided troubleshooting." },
                new DevTask { Id = 8, Area = "Continuous Delivery", Type = "New Responsibility", Description = "Implement blue-green deployment strategies.", Goal = "Implement with minimal guidance." },
                new DevTask { Id = 9, Area = "Continuous Delivery", Type = "New Responsibility", Description = "Set up comprehensive pipeline monitoring and deployment verification.", Goal = "Set up effective monitoring." },
                new DevTask { Id = 10, Area = "Continuous Delivery", Type = "Stretch Assignment", Description = "Modify workflows using GitHub Actions independently.", Goal = "Document improvements and manage end-to-end deployments." }
            };
        }
    }
}