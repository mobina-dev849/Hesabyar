using AutoMapper;
using Domain.Entities;
using Shared.Dtos;

namespace Service.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Person
            CreateMap<User, PersonDto>();
            CreateMap<PersonDto, User>();

            CreateMap<User, CreatePersonDto>();
            CreateMap<CreatePersonDto, User>();

            CreateMap<User, UpdatePersonDto>();
            CreateMap<UpdatePersonDto, User>();

            // Transaction
            CreateMap<Transaction, TransactionDto>()
                   .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                   .ForMember(dest => dest.CreatorPhoneNumber, opt => opt.MapFrom(src => src.Creator.PhoneNumber))
                   .ForMember(dest => dest.CounterpartyName, opt => opt.MapFrom(src => src.Counterparty.FullName))
                   .ForMember(dest => dest.CounterpartyPhoneNumber, opt => opt.MapFrom(src => src.Counterparty.PhoneNumber));

            CreateMap<TransactionDto, Transaction>();

            CreateMap<Transaction, CreateTransactionDto>();
            CreateMap<CreateTransactionDto, Transaction>();

            CreateMap<Transaction, UpdateTransacitonDto>();
            CreateMap<UpdateTransacitonDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StatusDate, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null));
        }
    }
}
