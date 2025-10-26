using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using System.Text.Json;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<TaskItem> _tasks = new List<TaskItem>();

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks()
        {
            return Ok(_tasks);
        }

        [HttpPost]
        public ActionResult<TaskItem> CreateTask([FromBody] TaskItem task)
        {
            task.Id = Guid.NewGuid();
            _tasks.Add(task);
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] JsonElement body)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == id);
            if (existingTask == null)
                return NotFound();

            if (body.TryGetProperty("isCompleted", out var isCompletedProp))
            {
                existingTask.IsCompleted = isCompletedProp.GetBoolean();
            }

            if (body.TryGetProperty("description", out var descriptionProp))
            {
                existingTask.Description = descriptionProp.GetString();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return NotFound();

            _tasks.Remove(task);
            return NoContent();
        }
    }
}
