using System;

namespace BudgetMate.Core.Entities;

public class Invoice
{
    public Guid Id { get; set; }
    public required string Path { get; set; }
    public Guid TransactionId { get; set; }
    public required Transaction Transaction { get; set; }
}
