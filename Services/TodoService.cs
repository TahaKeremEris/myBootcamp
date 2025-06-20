using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BootcampDay1.Services;        

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> GetUserTodosAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId) || _context.TodoItems == null)
            return new List<TodoItem>();

        return await _context.TodoItems
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<TodoItem?> GetTodoByIdAsync(int id, string userId)
    {
        return await _context.TodoItems
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<bool> CreateTodoAsync(TodoItem todo, string userId)
    {
        if (_context.TodoItems == null)
            return false;

        todo.UserId = userId;
        todo.CreatedDate = DateTime.Now;

        _context.TodoItems.Add(todo);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateTodoAsync(TodoItem todo, string userId)
{
    var existingTodo = await GetTodoByIdAsync(todo.Id, userId);
    if (existingTodo == null) return false;

    // Burada sadece güncellenebilir alanları kopyalıyoruz
    existingTodo.Title = todo.Title;
    existingTodo.IsCompleted = todo.IsCompleted;
    
    // İstersen başka alanları da ekleyebilirsin, örn: existingTodo.IsCompleted = todo.IsCompleted;

        try
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
}


    public async Task<bool> DeleteTodoAsync(int id, string userId)
    {
        var todo = await GetTodoByIdAsync(id, userId);
        if (todo == null) return false;

        _context.TodoItems.Remove(todo);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
