using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PersonRepository : BaseRepository<User>, IPersonRepository
    {
        public PersonRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<User?> GetByPhoneNumber(string phoneNumber, CancellationToken ct)
        {
            return await entities.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber, ct);
        }
    }
}
