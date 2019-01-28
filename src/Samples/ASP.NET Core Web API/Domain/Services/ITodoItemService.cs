using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuhRoh.Samples.WebAPI.Domain.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetAllTodoItems();
        Task<IEnumerable<TodoItem>> GetUncompletedTodoItems();
        Task<TodoItem> GetTodoItem(Guid id);

        Task<TodoItem> AddNewTodoItem(string description);
        Task MarkAsCompleted(Guid id);
    }
}