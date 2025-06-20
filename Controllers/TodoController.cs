using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

// eğer service kullanıyorsan onun namespace'i
using BootcampDay1.Services;
using BootcampDay1.Models;



[Authorize]
public class TodoController : Controller
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var todos = await _todoService.GetUserTodosAsync(userId!);
        return View(todos);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoItem model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid && userId != null)
        {
            var success = await _todoService.CreateTodoAsync(model, userId);
            if (success)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Görev eklenemedi.");
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var todo = await _todoService.GetTodoByIdAsync(id, userId!);
        if (todo == null) return NotFound();

        return View(todo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TodoItem model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid && userId != null)
        {
            var success = await _todoService.UpdateTodoAsync(model, userId);
            if (success)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Görev güncellenemedi.");
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var todo = await _todoService.GetTodoByIdAsync(id, userId!);
        if (todo == null) return NotFound();

        return View(todo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var success = await _todoService.DeleteTodoAsync(id, userId!);
        if (!success) return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
