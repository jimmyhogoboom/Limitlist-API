namespace limitlist_api.Models;

public class BudgetListItem
{
  public long Id { get; set; }
  public string Name { get; set; } = "";
  public decimal Price { get; set; }
}