using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RuhRoh.Samples.WebAPI.Domain;
using RuhRoh.Samples.WebAPI.Domain.Services;

namespace RuhRoh.Samples.WebAPI.Data.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly TodoDbContext _context;

        public TodoItemService(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodoItems()
        {
            return await _context.Set<TodoItem>()
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TodoItem>> GetUncompletedTodoItems()
        {
            return await _context.Set<TodoItem>()
                .AsNoTracking()
                .Where(x => !x.Completed)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public Task<TodoItem> GetTodoItem(Guid id)
        {
            return _context.Set<TodoItem>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TodoItem> AddNewTodoItem(string description)
        {
            var todoItem = new TodoItem { Description = description };
            _context.Add(todoItem);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return todoItem;
        }

        public async Task MarkAsCompleted(Guid id)
        {
            var todoItem = await _context.Set<TodoItem>()
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);

            if (todoItem != null)
            {
                todoItem.Completed = true;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}