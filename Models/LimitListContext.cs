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

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace limitlist_api.Models;

public class LimitListContext : IdentityDbContext
{
  public LimitListContext(DbContextOptions<LimitListContext> options) : base(options)
  {
  }

  public DbSet<BudgetListItem> BudgetListItems => Set<BudgetListItem>();
  public DbSet<BudgetList> BudgetLists => Set<BudgetList>();
}
