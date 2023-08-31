using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Route("api/TaskItems")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly TaskContext _context;

        public TaskItemsController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemTDO>>> GetTaskItems()
        {
            if (_context.TaskItems == null)
            {
                return NotFound();
            }
            return await _context.TaskItems.Select(x => ItemToTDO(x)).ToListAsync();
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemTDO>> GetTaskItem(long id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }
            return ItemToTDO(taskItem);
        }

        // PUT: api/TaskItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(long id, TaskItemTDO taskItemDTO)
        {
            if (id != taskItemDTO.Id)
            {
                return BadRequest();
            }
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.Title = taskItemDTO.Title;
            taskItem.Description = taskItemDTO.Description;
            taskItem.DueDate = taskItemDTO.DueDate;
            taskItem.IsCompleted = taskItemDTO.IsCompleted;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException) when (!TaskItemExists(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/TaskItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskItemTDO>> PostTaskItem(TaskItemTDO taskItemTDO)
        {
            var taskItem = new TaskItem
            {
                Title = taskItemTDO.Title,
                Description = taskItemTDO.Description,
                DueDate = taskItemTDO.DueDate,
                IsCompleted = taskItemTDO.IsCompleted,
            };
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTaskItem),
                new { id = taskItem.Id },
                ItemToTDO(taskItem));
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(long id)
        {
            if (_context.TaskItems == null)
            {
                return NotFound();
            }
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskItemExists(long id)
        {
            return (_context.TaskItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static TaskItemTDO ItemToTDO(TaskItem taskItem) =>
        new TaskItemTDO
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            DueDate = taskItem.DueDate,
            IsCompleted = taskItem.IsCompleted,
        };
    }
}
