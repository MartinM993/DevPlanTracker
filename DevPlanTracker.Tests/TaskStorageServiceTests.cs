using System.Text.Json;
using DevPlanTracker.Models;
using DevPlanTracker.Services;
using Microsoft.JSInterop;
using Moq;

namespace DevPlanTracker.Tests
{
    public class TaskStorageServiceTests
    {
        [Fact]
        public async Task LoadTasksAsync_WhenLocalStorageIsEmpty_ReturnsInitial10Tasks()
        {
            // Arrange
            var mockJsRuntime = new Mock<IJSRuntime>();
            
            // Tell the fake browser to return 'null' when asked for saved data (notice the string? fix!)
            mockJsRuntime
                .Setup(js => js.InvokeAsync<string?>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync((string?)null);

            var service = new TaskStorageService(mockJsRuntime.Object);

            // Act
            var tasks = await service.LoadTasksAsync();

            // Assert
            Assert.NotNull(tasks);
            Assert.Equal(11, tasks.Count);
            Assert.Equal("Delivery Excellence", tasks[0].Area);
        }

        [Fact]
        public async Task LoadTasksAsync_WhenDataExists_ReturnsSavedTasks()
        {
            // Arrange: Create some fake saved data
            var mockJsRuntime = new Mock<IJSRuntime>();
            var savedTasks = new List<DevTask> 
            { 
                new DevTask { Id = 99, Area = "Custom Tracker", Description = "My custom saved task" } 
            };
            var savedJson = JsonSerializer.Serialize(savedTasks);

            // Tell the browser to return our custom JSON instead of null
            mockJsRuntime
                .Setup(js => js.InvokeAsync<string?>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(savedJson);

            var service = new TaskStorageService(mockJsRuntime.Object);

            // Act
            var tasks = await service.LoadTasksAsync();

            // Assert: Prove it loaded the 1 custom task, NOT the 10 defaults
            Assert.NotNull(tasks);
            Assert.Single(tasks); 
            Assert.Equal(99, tasks[0].Id);
            Assert.Equal("My custom saved task", tasks[0].Description);
        }

        [Fact]
        public async Task GetEditorHtmlAsync_CallsJavascript_WithCorrectElementId()
        {
            // Arrange
            var mockJsRuntime = new Mock<IJSRuntime>();
            var expectedHtml = "<b>My rich text notes</b>";
            var taskId = 5;

            // Setup the mock to specifically look for the correct editor ID ("editor-5")
            mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("getEditorHtml", It.Is<object[]>(args => (string)args[0] == $"editor-{taskId}")))
                .ReturnsAsync(expectedHtml);

            var service = new TaskStorageService(mockJsRuntime.Object);

            // Act
            var result = await service.GetEditorHtmlAsync(taskId);

            // Assert: Prove the service successfully retrieved the HTML
            Assert.Equal(expectedHtml, result);
        }
    }
}