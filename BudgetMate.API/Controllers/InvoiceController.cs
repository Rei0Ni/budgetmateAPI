using System.Security.Claims;
using BudgetMate.Application.Interfaces.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        [Route("{InvoiceId}")]
        public async Task<ActionResult> GetInvoiceAsync(string InvoiceId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var Invoice = await _service.GetInvoiceAsync(userId, InvoiceId);

            if (Invoice == null){
                return NoContent();
            }
            return File(Invoice, "image/jpg");
        }
    }
}
