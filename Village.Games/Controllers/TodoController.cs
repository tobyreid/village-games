using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Queue;
using Village.Games.Models;

namespace Village.Games.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : Controller
    {
        private readonly TodoDbContext _dbContext;
        private readonly IQueueResolver _queueResolver;
        public TodoController(TodoDbContext dbContext, IQueueResolver queueResolver)
        {
            _dbContext = dbContext;
            _queueResolver = queueResolver;

            if (_dbContext.TodoItems.Count() == 0)
            {
                _dbContext.TodoItems.Add(new TodoItem { Name = "Item1" });
                _dbContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            await _queueResolver.GetQueue(AzureQueues.EmailQueue).AddMessageAsync(new CloudQueueMessage("{Name: \"Toby\"}"));
            return await _dbContext.TodoItems.ToListAsync();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var item = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new  ObjectResult(item);
        }
        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly-created TodoItem</returns>
        /// <response code="201">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>     
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(typeof(TodoItem), 400)]
        public async Task<IActionResult> CreateAsync([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _dbContext.TodoItems.Add(item);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _dbContext.TodoItems.Update(todo);
            await _dbContext.SaveChangesAsync();
            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific TodoItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var todo = await  _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _dbContext.TodoItems.Remove(todo);
            await _dbContext.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
