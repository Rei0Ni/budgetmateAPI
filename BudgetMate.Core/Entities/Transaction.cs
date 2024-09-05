using System;

namespace BudgetMate.Core.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal amount { get; set; }
    public TransactionCategory Category { get; set; }
    public string Description { get; set; } = "";
    public DateTime Date { get; set; }

    public Guid UserId { get; set; }
    public required ApplicationUser User { get; set; }
}

public enum TransactionType
{
    INCOME = 0,
    EXPENSE = 1
}

public enum TransactionCategory
{
    INCOME_SALARY = 0,
    INCOME_BONUS =1,
    INCOME_FREELANCING = 2,
    INCOME_INVESTMENT_GAINS = 3,
    INCOME_RENTAL_INCOME = 4,
    INCOME_REFUNDS = 5,
    INCOME_GRANT = 6,

    EXPENSE_RENT = 7,
    EXPENSE_UTILITIES = 8,
    EXPENSE_GROCERIES = 9,
    EXPENSE_TRANSPORTATION = 10,
    EXPENSE_INSURANCE = 11,
    EXPENSE_HEALTHCARE = 12,
    EXPENSE_ENTERTAINMENT = 13,
    EXPENSE_SHOPPING = 14,
    EXPENSE_DEPT_PAYMENT = 15,
    EXPENSE_INVESTMENT = 16,
    EXPENSE_TAXES = 17,
    EXPENSE_DONATION = 18,
}