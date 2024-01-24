using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using limitlist_api.Models;

namespace limitlist_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BudgetItemsController : ControllerBase
  {
    private readonly LimitListContext _context;

    public BudgetItemsController(LimitListContext context)
    {
      _context = context;
    }

    // GET: api/BudgetItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetListItem>>> GetBudgetListItems()
    {
      return await _context.BudgetListItems.ToListAsync();
    }

    // GET: api/BudgetItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetListItem>> GetBudgetListItem(long id)
    {
      var budgetListItem = await _context.BudgetListItems.FindAsync(id);

      if (budgetListItem == null)
      {
        return NotFound();
      }

      return budgetListItem;
    }

    // PUT: api/BudgetItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBudgetListItem(long id, BudgetListItem budgetListItem)
    {
      if (id != budgetListItem.Id)
      {
        return BadRequest();
      }

      _context.Entry(budgetListItem).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!BudgetListItemExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/BudgetItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BudgetListItem>> PostBudgetListItem(BudgetListItem budgetListItem)
    {
      _context.BudgetListItems.Add(budgetListItem);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetBudgetListItem), new { id = budgetListItem.Id }, budgetListItem);
    }

    // DELETE: api/BudgetItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudgetListItem(long id)
    {
      var budgetListItem = await _context.BudgetListItems.FindAsync(id);
      if (budgetListItem == null)
      {
        return NotFound();
      }

      _context.BudgetListItems.Remove(budgetListItem);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool BudgetListItemExists(long id)
    {
      return _context.BudgetListItems.Any(e => e.Id == id);
    }
  }
}
