using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Shared.Dtos;
using Service.Interfaces;
using System.Collections.Generic;

namespace Service
{
    public class PersonService : IPersonService
    {
        private readonly IBaseRepository<User> _baseRepository;
        private readonly IMapper _mapper;

        public PersonService(IBaseRepository<User> baseRepository, IMapper mapper)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
        }

        public async Task Add(CreatePersonDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<User>(dto);
            await _baseRepository.Add(entity, ct);
        }

        public async Task Delete(long id, CancellationToken ct)
        {
            await _baseRepository.Delete(id, ct);
        }

        public async Task<List<PersonDto>> GetAll(CancellationToken ct)
        {
            var persons = await _baseRepository.GetAll(ct); // Entity ها رو می‌گیریم
            return _mapper.Map<List<PersonDto>>(persons);   // اینجا تبدیل به DTO می‌کنیم
        }

        public async Task<PersonDto> GetById(long id, CancellationToken ct)
        {
            var person = await _baseRepository.GetById(id, ct);

            if (person == null)
                throw new KeyNotFoundException("کاربر مورد نظر پیدا نشد!");

            return _mapper.Map<PersonDto>(person);
        }

        public async Task Update(PersonDto dto, CancellationToken ct)
        {
            var person = await _baseRepository.GetById(dto.Id, ct);

            if (person == null)
                throw new KeyNotFoundException("کاربر مورد نظر پیدا نشد!");

            _mapper.Map(dto, person);
            await _baseRepository.Update(person, ct);
        }
    }
}