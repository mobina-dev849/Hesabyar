using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Transaction?> GetByIdWithDetails(long id, CancellationToken ct)
        {
            return await entities
                .Include(t => t.Creator)
                .Include(t => t.Counterparty)
                .FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        public async Task<List<Transaction>> GetUserTransactions(long userId, CancellationToken ct)
        {
            return await entities
                .Include(t => t.Creator)
                .Include(t => t.Counterparty)
                .Where(t =>
                    t.CreatorId == userId ||
                    t.CounterpartyId == userId)
                .ToListAsync(ct);
        }
    }
}