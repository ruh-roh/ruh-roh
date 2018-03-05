using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RuhRoh.Samples.WebAPI.Controllers.Models;
using RuhRoh.Samples.WebAPI.Domain.Services;

namespace RuhRoh.Samples.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("todoitems")]
    public class TodoItemController : Controller
    {
        private readonly ITodoItemService _service;

        public TodoItemController(ITodoItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoItems([FromQuery]bool uncompletedOnly = false)
        {
            var items = uncompletedOnly ? 
                await _service.GetUncompletedTodoItems() :
                await _service.GetAllTodoItems();

            return Ok(items);
        }

        [HttpGet("{id:guid}", Name = nameof(GetTodoItemById))]
        public async Task<IActionResult> GetTodoItemById(Guid id)
        {
            var item = await _service.GetTodoItem(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem([FromBody]CreateTodoItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _service.AddNewTodoItem(model.Description);
            return CreatedAtRoute(nameof(GetTodoItemById), new {item.Id}, item);
        }

        [HttpPut("{id:guid}/done")]
        public async Task<IActionResult> MarkItemAsCompleted(Guid id)
        {
            var item = await _service.GetTodoItem(id);
            if (item == null)
            {
                return NotFound();
            }

            if (item.Completed)
            {
                return NoContent();
            }

            await _service.MarkAsCompleted(id);
            return NoContent();
        }
    }
}