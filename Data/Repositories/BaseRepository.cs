using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DbSet<TEntity> entities{ get; }
        

        public BaseRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
            entities= context.Set<TEntity>();
        }

        public async Task Add(TEntity entity, CancellationToken ct, bool saveNow = true)
        {
            await entities.AddAsync(entity, ct);
            if(saveNow)
                await _context.SaveChangesAsync(ct);
        }

        public async Task Delete(long id, CancellationToken ct, bool saveNow = true)
        {
            var tEntity = await entities.FindAsync(id);
            entities.Remove(tEntity);
            if(saveNow) 
               await _context.SaveChangesAsync(ct);
        }

        public async Task<List<TEntity>> GetAll(CancellationToken ct)
        {
            return await entities.ToListAsync(ct);
        }

        public async Task<TEntity> GetById(long id, CancellationToken ct)
        {
            return await entities.FindAsync(id,ct);

        }

        public async Task Update(TEntity entity, CancellationToken ct, bool saveNow = true)
        {
             entities.Update(entity); //await
            if(saveNow)
                await _context.SaveChangesAsync(ct);
        }
    }
}
