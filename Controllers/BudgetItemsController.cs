/*
 * Copyright 2024 Jimmy Hogoboom
 *
 * This file is part of the LimitList API.
 *
 * LimitList API is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *
 * LimitList API is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with LimitList API. If not, see <https://www.gnu.org/licenses/>.
 */

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
