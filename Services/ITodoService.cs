using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampDay1.Models;

namespace BootcampDay1.Services
{
    public interface ITodoService
    {
        Task<List<TodoItem>> GetUserTodosAsync(string userId);
        Task<TodoItem?> GetTodoByIdAsync(int id, string userId);
        Task<bool> CreateTodoAsync(TodoItem todo, string userId);
        Task<bool> UpdateTodoAsync(TodoItem todo, string userId);
        Task<bool> DeleteTodoAsync(int id, string userId);
    }
}
