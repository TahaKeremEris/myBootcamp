

using AutoMapper;
using BootcampDay1.DTOs;
using BootcampDay1.Models;
using BootcampDay1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BootcampDay1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoApiController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IMapper _mapper;

        public TodoApiController(ITodoService todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        // GET: api/todoapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todos = await _todoService.GetUserTodosAsync(userId!);
            var todosDto = _mapper.Map<List<TodoItemDto>>(todos);
            return Ok(todosDto);
        }

        // GET: api/todoapi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = await _todoService.GetTodoByIdAsync(id, userId!);
            if (todo == null)
                return NotFound();

            var todoDto = _mapper.Map<TodoItemDto>(todo);
            return Ok(todoDto);
        }

        // POST: api/todoapi
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TodoItemDto todoDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = _mapper.Map<TodoItem>(todoDto);
            var result = await _todoService.CreateTodoAsync(todo, userId!);
            if (!result)
                return BadRequest("Görev oluşturulamadı.");

            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todoDto);
        }

        // PUT: api/todoapi/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TodoItemDto todoDto)
        {
            if (id != todoDto.Id)
                return BadRequest("ID uyuşmuyor.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = _mapper.Map<TodoItem>(todoDto);
            var result = await _todoService.UpdateTodoAsync(todo, userId!);

            if (!result)
                return NotFound("Görev bulunamadı veya güncellenemedi.");

            return NoContent();
        }

        // DELETE: api/todoapi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _todoService.DeleteTodoAsync(id, userId!);
            if (!result)
                return NotFound("Görev bulunamadı veya silinemedi.");

            return NoContent();
        }
    }
}
