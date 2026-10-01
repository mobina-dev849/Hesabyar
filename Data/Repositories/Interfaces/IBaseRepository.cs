using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        DbSet<TEntity> entities { get; }
        Task<List<TEntity>> GetAll(CancellationToken ct);

        Task<TEntity> GetById(long id, CancellationToken ct);

        Task Add(TEntity entity, CancellationToken ct, bool saveNow = true);

        Task Update(TEntity entity, CancellationToken ct, bool saveNow = true);

        Task Delete(long id, CancellationToken ct, bool saveNow = true);

        

    }
}
