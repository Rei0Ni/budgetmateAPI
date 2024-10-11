using System;

namespace BudgetMate.Application.DTO.Transaction;

public class AllTransactionsDto
{
    public List<TransactionDto>? Transactions { get; set; }
}
