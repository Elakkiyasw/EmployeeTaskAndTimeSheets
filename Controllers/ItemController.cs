using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WEBAPI_Demo.Models;
using WEBAPI_Demo.Repository;

namespace WEBAPI_Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly JsonFlatFileRepository _repository;
        private readonly ILogger<ItemController> _logger;
        public ItemController(ILogger<ItemController> logger)
        {
            _repository = new JsonFlatFileRepository("employees.json");
            _logger = logger;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get(int pageNumber = 1, int pageSize = 100)
        {
            _logger.LogInformation("Getting items for page {PageNumber} with size {PageSize}", pageNumber, pageSize);
            var items = _repository.GetAll(pageNumber, pageSize);
            var totalCount = _repository.GetTotalCount();
            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = items
            });
        }

        [HttpGet("{id}")]
        public ActionResult<Item> Get(int id)
        {
            _logger.LogInformation("Getting item with ID {ItemId}", id);
            var item = _repository.Get(id);
            if (item == null)
            {
                _logger.LogWarning("Item with ID {ItemId} not found", id);
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Item employee)
        {
            _repository.Add(employee);
            _logger.LogInformation("Added new item with ID {ItemId}", employee.ID);
            return CreatedAtAction(nameof(Get), new { id = employee.ID }, employee);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Item item)
        {
            if (id != item.ID)
            {
                _logger.LogWarning("Item ID mismatch: {ItemId}", id);
                return BadRequest();
            }
            var existingItem = _repository.Get(id);
            if (existingItem == null)
            {
                _logger.LogWarning("Item with ID {ItemId} not found for update", id);
                return NotFound();

            }

            item.EmpID = id;
            _repository.Update(item);
            _logger.LogInformation("Updated item with ID {ItemId}", item.ID);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingItem = _repository.Get(id);
            if (existingItem == null)
            { 
                _logger.LogWarning("Item with ID {ItemId} not found for deletion", id);
            return NotFound();
            }
            _repository.Delete(id);
            _logger.LogInformation("Deleted item with ID {ItemId}", id);
            return NoContent();
        }
    }
}
