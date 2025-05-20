using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEBAPI_Demo.Models;

namespace WEBAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly string filePath = "Employee_Reports.txt";

        public TasksController()
        {
            // Ensure the file exists when the application starts
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.WriteAllText(filePath, "[]");
            }
        }

        private List<TaskItem> ReadTasksFromFile()
        {
            var fileContent = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<TaskItem>>(fileContent);
        }

        private void WriteTasksToFile(List<TaskItem> tasks)
        {
            var jsonContent = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, jsonContent);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var tasks = ReadTasksFromFile();
            return Ok(tasks);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TaskItem task)
        {
            var tasks = ReadTasksFromFile();
            task.Id = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
            tasks.Add(task);
            WriteTasksToFile(tasks);
            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TaskItem updatedTask)
        {
            var tasks = ReadTasksFromFile();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            task.EmployeeName = updatedTask.EmployeeName;
            task.TaskDescription = updatedTask.TaskDescription;
            task.Status = updatedTask.Status;

            WriteTasksToFile(tasks);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tasks = ReadTasksFromFile();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            tasks.Remove(task);
            WriteTasksToFile(tasks);
            return NoContent();
        }
    }
}

