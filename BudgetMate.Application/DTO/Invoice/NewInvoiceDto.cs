using System;
using Microsoft.AspNetCore.Http;

namespace BudgetMate.Application.DTO.Invoice;

public class NewInvoiceDto
{
    public required Core.Entities.Transaction Transaction { get; set; }
    public required IFormFile File { get; set; }
}
