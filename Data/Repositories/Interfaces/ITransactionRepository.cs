using Domain.Entities;

namespace Data.Repositories.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<Transaction?> GetByIdWithDetails(long id, CancellationToken ct);

        Task<List<Transaction>> GetUserTransactions(long userId, CancellationToken ct);
    }
}