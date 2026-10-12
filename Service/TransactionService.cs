using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Shared.Dtos;
using Service.Interfaces;
using System;
using System.Collections.Generic;

namespace Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository,
            IPersonRepository personRepository, IMapper mapper)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _personRepository = personRepository;
        }

        public async Task Add(long creatorId, CreateTransactionDto dto, CancellationToken ct)
        {
            var creator = await _personRepository.GetById(creatorId, ct);
            if (creator == null)
                throw new KeyNotFoundException("کاربر پیدا نشد!");

            var counterparty = await _personRepository.GetByPhoneNumber(dto.CounterpartyPhoneNumber, ct);
            if (counterparty == null)
                throw new KeyNotFoundException("طرف حساب با این شماره تلفن پیدا نشد!");

            var transaction = _mapper.Map<Transaction>(dto);
            transaction.CreatorId = creator.Id;
            transaction.Creator = creator;
            transaction.CounterpartyId = counterparty.Id;
            transaction.Status = Transaction.TransactionStatus.Pending;
            transaction.StatusDate = null;
            transaction.TransactionDate = DateTime.UtcNow;

            await _transactionRepository.Add(transaction, ct);
        }

        public async Task Delete(long id, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetById(id, ct);

            if (transaction == null)
                throw new KeyNotFoundException("تراکنش مورد نظر پیدا نشد!");

            if (transaction.Status == Transaction.TransactionStatus.Pending)
                throw new InvalidOperationException("تراکنش های در حال انتظار قابل حذف نیستند.");

            await _transactionRepository.Delete(id, ct);
        }

        public async Task Update(long id, UpdateTransacitonDto dto, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetById(id, ct);

            if (transaction == null)
                throw new KeyNotFoundException("تراکنش مورد نظر پیدا نشد!");

            if (transaction.Status != Transaction.TransactionStatus.Pending)
                throw new InvalidOperationException("فقط تراکنش های در حال انتظار قابل ویرایش هستند.");

            _mapper.Map(dto, transaction);

            await _transactionRepository.Update(transaction, ct);
        }

        public async Task<TransactionDto> GetById(long id, long requestingUserId, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetByIdWithDetails(id, ct);

            if (transaction == null)
                throw new KeyNotFoundException("تراکنش مورد نظر پیدا نشد!");

            if (transaction.CreatorId != requestingUserId && transaction.CounterpartyId != requestingUserId)
                throw new UnauthorizedAccessException("شما به این تراکنش دسترسی ندارید.");

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            

            return transactionDto;
        }

        public async Task Approve(long transactionId, long personId, CancellationToken ct)
            => await ChangeStatus(transactionId, personId, Transaction.TransactionStatus.Approved, ct);

        public async Task Reject(long transactionId, long personId, CancellationToken ct)
            => await ChangeStatus(transactionId, personId, Transaction.TransactionStatus.Rejected, ct);

        private async Task ChangeStatus(long transactionId, long personId, Transaction.TransactionStatus newStatus, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetById(transactionId, ct);
            if (transaction == null)
                throw new KeyNotFoundException("تراکنش پیدا نشد.");

            var person = await _personRepository.GetById(personId, ct);
            if (person == null)
                throw new KeyNotFoundException("کاربر پیدا نشد.");

            if (person.Id != transaction.CounterpartyId)
                throw new UnauthorizedAccessException("شما مجاز به تغییر وضعیت این تراکنش نیستید.");

            if (transaction.Status != Transaction.TransactionStatus.Pending)
                throw new InvalidOperationException("فقط تراکنش های در حال انتظار قابل تغییر وضعیت هستند.");

            transaction.Status = newStatus;
            transaction.StatusDate = DateTime.UtcNow;

            await _transactionRepository.Update(transaction, ct);
        }

        public async Task<List<TransactionDto>> GetUserTransactions(long userId, CancellationToken ct)
        {
            var transactions = await _transactionRepository.GetUserTransactions(userId, ct);

            var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

            return transactionDtos;
        }

        public async Task<List<AccountDto>> GetUserAccounts(long userId, CancellationToken ct)
        {
            var transactions = await _transactionRepository.GetUserTransactions(userId, ct);
            var accountsByPerson = new Dictionary<long, AccountDto>();

            foreach (var transaction in transactions)
            {
                var userIsCreator = transaction.CreatorId == userId;
                var personId = userIsCreator ? transaction.CounterpartyId : transaction.CreatorId;
                var personName = userIsCreator
                    ? transaction.Counterparty.FullName
                    : transaction.Creator.FullName;

                if (!accountsByPerson.TryGetValue(personId, out var account))
                {
                    account = new AccountDto
                    {
                        PersonId = personId,
                        FullName = personName,
                        Balance = 0
                    };
                    accountsByPerson.Add(personId, account);
                }

                // Temporarily include all transaction statuses in the account balance.
                // if (transaction.Status != Transaction.TransactionStatus.Approved)
                //     continue;

                var isPayment = transaction.Type == Transaction.TransactionType.Payment;
                var increasesBalance = userIsCreator == isPayment;
                account.Balance += increasesBalance ? transaction.Amount : -transaction.Amount;
            }

            return accountsByPerson.Values.ToList();
        }
    }
}
