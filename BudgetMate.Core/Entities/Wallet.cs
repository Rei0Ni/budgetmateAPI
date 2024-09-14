using System;

namespace BudgetMate.Core.Entities;

public class Wallet
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public Guid UserId { get; set; }
    public required ApplicationUser User { get; set; }

}
