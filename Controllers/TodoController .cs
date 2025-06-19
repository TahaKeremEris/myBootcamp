using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class TodoController : Controller
{
    private readonly AppDbContext _context;

    public TodoController(AppDbContext context)
    {
        _context = context;
    }

    // Listeleme - sadece giriş yapan kullanıcının görevleri
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.TodoItems == null || userId == null)
        {
            return View(new List<TodoItem>());
        }

        var items = await _context.TodoItems
            .Where(t => t.UserId == userId)
            .ToListAsync();

        return View(items);
    }

    // Create - GET
    public IActionResult Create()
    {
        return View();
    }

    // Create - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoItem model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (ModelState.IsValid && userId != null)
        {
            model.CreatedDate = DateTime.Now;
            model.UserId = userId;

            if (_context.TodoItems != null)
            {
                _context.TodoItems.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "TodoItems DbSet is null.");
        }

        return View(model);
    }

    // Edit - GET
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var item = await _context.TodoItems
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (item == null) return NotFound();

        return View(item);
    }

    // Edit - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TodoItem model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var item = await _context.TodoItems
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == model.Id && t.UserId == userId);

        if (item == null) return NotFound();

        if (ModelState.IsValid)
        {
            model.UserId = userId; // Kullanıcıyı koru
            model.CreatedDate = item.CreatedDate; // Tarihi bozma

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        return View(model);
    }

    // Delete - GET
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var item = await _context.TodoItems
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (item == null) return NotFound();

        return View(item);
    }

    // Delete - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var item = await _context.TodoItems
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (item == null) return NotFound();

        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
