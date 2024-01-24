using Microsoft.EntityFrameworkCore;

namespace limitlist_api.Models;

public class LimitListContext : DbContext
{
  public LimitListContext(DbContextOptions<LimitListContext> options) : base(options)
  {
  }

  public DbSet<BudgetListItem> BudgetListItems { get; set; } = null!;
}