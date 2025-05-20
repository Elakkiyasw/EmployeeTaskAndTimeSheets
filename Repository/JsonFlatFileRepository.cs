using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using WEBAPI_Demo.Models;
using System.Text.Json;

namespace WEBAPI_Demo.Repository
{
    public class JsonFlatFileRepository
    {
        private readonly string _filePath;

        public JsonFlatFileRepository(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]"); // Initialize file with an empty array
            }
        }

        private List<Item> LoadItems()
        {
            var json = File.ReadAllText(_filePath);
            return System.Text.Json.JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();
        }
        private void SaveItems(List<Item> items)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(items);
            File.WriteAllText(_filePath, json);
        }
        public IEnumerable<Item> GetAll(int pageNumber, int pageSize)
        {
            var items = LoadItems();
            return items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        public int GetTotalCount()
        {

            return LoadItems().Count;
        }
        public Item Get(int id) => LoadItems().FirstOrDefault(i => i.ID == id);

        

        
        public void Add(Item employee)
        {
            var employees = LoadItems();
            int newID = employees.Any() ? employees.Max(x => x.ID) + 1 : 1;
            employee.ID = newID;
            employees.Add(employee);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented));
        }

        public void Update(Item employee)
        {
            var employees = LoadItems();
            var index = employees.FindIndex(x => x.ID == employee.ID);
            if (index >= 0)
            {
                employees[index] = employee;
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented));
            }
        }

        public void Delete(int id)
        {
            var employees = LoadItems();
            var employeeToRemove = employees.FirstOrDefault(x => x.ID == id);
            if (employeeToRemove != null)
            {
                employees.Remove(employeeToRemove);
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented));
            }
        }
    }
}
