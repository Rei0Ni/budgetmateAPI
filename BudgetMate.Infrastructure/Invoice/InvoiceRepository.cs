using System;
using BudgetMate.Application.Interfaces.Invoice;
using BudgetMate.Core.Contexts;
using MongoDB.Driver;

namespace BudgetMate.Infrastructure.Invoice;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDBContext _context;

    public InvoiceRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public Core.Entities.Invoice AddInvoice(Core.Entities.Invoice Invoice)
    {
        _context.Invoices.InsertOne(Invoice);
        return Invoice;
    }

    public Core.Entities.Invoice GetByTransactionId(string TransactionId)
    {
        var Invoice = _context.Invoices.Find(I => I.TransactionId == new Guid(TransactionId))
                .FirstOrDefault();
        return Invoice;
    }
}
