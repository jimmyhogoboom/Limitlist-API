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
using Microsoft.IdentityModel.Tokens;

namespace limitlist_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BudgetListItemsController : ControllerBase
  {
    private readonly LimitListContext _context;

    public BudgetListItemsController(LimitListContext context)
    {
      _context = context;
    }

    // GET: api/BudgetItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetListItemDTO>>> GetBudgetListItems()
    {
      return await _context.BudgetListItems
        .Select(x => ItemToDTO(x))
        .ToListAsync();
    }

    // GET: api/BudgetItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetListItemDTO>> GetBudgetListItem(Guid id)
    {
      var budgetListItem = await _context.BudgetListItems.FindAsync(id);

      if (budgetListItem == null)
      {
        return NotFound();
      }

      return ItemToDTO(budgetListItem);
    }

    // PUT: api/BudgetItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBudgetListItem(Guid id, BudgetListItemDTO budgetListItemDTO)
    {
      if (id != budgetListItemDTO.Id)
      {
        return BadRequest();
      }

      var item = await _context.BudgetListItems.FindAsync(id);
      if (item == null)
      {
        return NotFound();
      }

      ApplyDTOToItem(ref item, budgetListItemDTO);

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) when (!BudgetListItemExists(id))
      {
        return NotFound();
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
    public async Task<IActionResult> DeleteBudgetListItem(Guid id)
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

    private bool BudgetListItemExists(Guid id)
    {
      return _context.BudgetListItems.Any(e => e.Id == id);
    }

    private static BudgetListItemDTO ItemToDTO(BudgetListItem budgetListItem) =>
      new BudgetListItemDTO
      {
        Id = budgetListItem.Id,
        Name = budgetListItem.Name,
        Price = budgetListItem.Price,
        Position = budgetListItem.Position,
      };

    private static void ApplyDTOToItem(ref BudgetListItem budgetListItem, BudgetListItemDTO budgetListItemDTO)
    {
      budgetListItem.Id = budgetListItemDTO.Id;
      budgetListItem.Name = budgetListItemDTO.Name.IsNullOrEmpty() ? budgetListItemDTO.Name : budgetListItem.Name;
      budgetListItem.Price = budgetListItemDTO.Price;
      budgetListItem.Position = budgetListItemDTO.Position;
    }
  }
}
