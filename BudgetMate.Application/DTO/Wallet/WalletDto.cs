using System;
using BudgetMate.Application.DTO.Transaction;

namespace BudgetMate.Application.DTO.Wallet;

public class WalletDto
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }

}
