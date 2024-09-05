using System.Security.Claims;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.Interfaces.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("Get/{ID}")]
        public ActionResult<TransactionDto> Get(string ID)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var transaction = _transactionService.GetTransaction(ID, userId);
            return Ok(transaction);
        }

        [HttpGet]
        [Route("Get/All")]
        public ActionResult<List<TransactionDto>> GetAll()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var transactions = _transactionService.GetAllUserTransaction(userId);
            return Ok(transactions);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<List<TransactionDto>>> AddNew(NewTransactionDto dto)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var transactions = await _transactionService.AddTransactionAsync(dto, userId);
            return Ok(transactions);
        }

        [HttpPost]
        [Route("Delete/{ID}")]
        public ActionResult<List<TransactionDto>> Delete(string ID)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var transactions = _transactionService.DeleteTransaction(ID, userId);
            return Ok(transactions);
        }
    }
}
