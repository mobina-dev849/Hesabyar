using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTransactions(CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            if (!long.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var transactions = await _transactionService.GetUserTransactions(userId, ct);

            return Ok(transactions);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var transaction = await _transactionService.GetById(id, userId, ct);

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateTransactionDto dto,CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var creatorId = long.Parse(userIdClaim.Value);

            await _transactionService.Add(creatorId, dto, ct);

            return Ok();
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, UpdateTransacitonDto dto, CancellationToken ct)
        {
            //if (id != dto.Id)
            //    throw new ArgumentException("شناسه تراکنش صحیح نیست.");
            await _transactionService.Update(id, dto, ct);
            return Ok();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            await _transactionService.Delete(id, ct);
            return Ok();
        }

        [HttpPost("{id:long}/approve")]
        public async Task<IActionResult> Approve(long id,long userId, CancellationToken ct) 
        {
            await _transactionService.Approve(id, userId, ct);
            return Ok();
        }

        [HttpPost("{id:long}/reject")]
        public async Task<IActionResult> Reject(long id, long userId, CancellationToken ct)
        {
            await _transactionService.Reject(id, userId, ct);
            return Ok();
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetUserAccounts(CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var accounts = await _transactionService.GetUserAccounts(userId, ct);

            return Ok(accounts);
        }


    }

}
