using System;
using BudgetMate.Application.DTO.Invoice;
using BudgetMate.Application.DTO.Transaction;
using Microsoft.AspNetCore.Http;

namespace BudgetMate.Application.Interfaces.Invoice;

public interface IInvoiceService
{
    Task<Byte[]?> GetInvoiceAsync(string UserId, string TransactionId);
    string? AddInvoice(IFormFile Invoice, Core.Entities.Transaction transaction);
}
