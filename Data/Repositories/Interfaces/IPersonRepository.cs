using Domain.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IPersonRepository : IBaseRepository<User>
    {
        Task<User?> GetByPhoneNumber(string phoneNumber, CancellationToken ct);
    }
}