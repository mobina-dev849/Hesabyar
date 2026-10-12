using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
namespace Service.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> GetById(long id, long requestingUserId, CancellationToken ct);

        Task Add(long creatorId, CreateTransactionDto dto, CancellationToken ct);

        Task Update(long id, UpdateTransacitonDto dto, CancellationToken ct);

        Task Delete(long id, CancellationToken ct);

        Task Approve(long transactionId,long personId,CancellationToken ct);

        Task Reject(long transactionId, long personId, CancellationToken ct);

        Task<List<TransactionDto>> GetUserTransactions(long userId, CancellationToken ct);

        Task<List<AccountDto>> GetUserAccounts(long userId, CancellationToken ct);
    }
}
