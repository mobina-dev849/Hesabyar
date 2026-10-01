using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IPersonService
    {

        Task<List<PersonDto>> GetAll(CancellationToken ct);
      
        Task<PersonDto> GetById(long id, CancellationToken ct);

        Task Add(CreatePersonDto dto, CancellationToken ct);

        Task Update(PersonDto dto, CancellationToken ct);

        Task Delete(long id, CancellationToken ct);
    }
}
