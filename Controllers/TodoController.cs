using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
  private readonly ApiDbContext _context;
  public TodoController(ApiDbContext context)
  {
    _context = context;
  }
  [HttpGet]
  public async Task<IActionResult> GetItems()
  {
    var items = await _context.Items.ToListAsync();
    return Ok(items);
  }
  [HttpPost]
  public async Task<IActionResult> CreateItem(ItemData data)
  {
    if (ModelState.IsValid)
    {
      await _context.Items.AddAsync(data);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetItem", new { data.Id }, data);
    }

    return new JsonResult("Something went wrong") { StatusCode = 500 };
  }
  [HttpGet("{id}")]
  public async Task<IActionResult> GetItem(int id)
  {
    var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

    if (item == null)
      return NotFound();

    return Ok(item);
  }
}

