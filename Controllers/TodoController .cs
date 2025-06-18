using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class TodoController : Controller
{
    private readonly AppDbContext _context;

    public TodoController(AppDbContext context)
    {
        _context = context;
    }

    // Listeleme
    public async Task<IActionResult> Index()
    {
        var items = await _context.TodoItems.ToListAsync();
        Console.WriteLine("Veritabanındaki toplam kayıt: " + items.Count); // Konsola yazdırır
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
        if (ModelState.IsValid)
        {
            model.CreatedDate = DateTime.Now;
            _context.TodoItems.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // Edit - GET
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Edit - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TodoItem model)
    {
        if (ModelState.IsValid)
        {
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

    // Delete - GET (onay sayfası)
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    // Delete - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return NotFound();

        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}