using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using limitlist_api.Models;

namespace limitlist_api
{
  [Route("api/[controller]")]
  [ApiController]
  public class BudgetListsController : ControllerBase
  {
    private readonly LimitListContext _context;

    public BudgetListsController(LimitListContext context)
    {
      _context = context;
    }

    // GET: api/BudgetLists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetList>>> GetBudgetLists()
    {
      // TODO: Use DTO to include items while avoiding a cycle
      return await _context.BudgetLists.ToListAsync();
    }

    // GET: api/BudgetLists/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetList>> GetBudgetList(Guid id)
    {
      var budgetList = await _context.BudgetLists.FindAsync(id);

      if (budgetList == null)
      {
        return NotFound();
      }

      return budgetList;
    }

    // PUT: api/BudgetLists/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBudgetList(Guid id, BudgetList budgetList)
    {
      if (id != budgetList.Id)
      {
        return BadRequest();
      }

      _context.Entry(budgetList).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!BudgetListExists(id))
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

    // POST: api/BudgetLists
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BudgetList>> PostBudgetList(BudgetList budgetList)
    {
      // TODO: save logged in user on list as owner

      _context.BudgetLists.Add(budgetList);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetBudgetList", new { id = budgetList.Id }, budgetList);
    }

    // DELETE: api/BudgetLists/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudgetList(Guid id)
    {
      var budgetList = await _context.BudgetLists.FindAsync(id);
      if (budgetList == null)
      {
        return NotFound();
      }

      _context.BudgetLists.Remove(budgetList);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool BudgetListExists(Guid id)
    {
      return _context.BudgetLists.Any(e => e.Id == id);
    }
  }
}
