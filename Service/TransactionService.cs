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

        public async Task<TransactionDto> GetById(long id, CancellationToken ct)
        {
            var transaction = await _transactionRepository.GetByIdWithDetails(id, ct);

            if (transaction == null)
                throw new KeyNotFoundException("تراکنش مورد نظر پیدا نشد!");

            return _mapper.Map<TransactionDto>(transaction);
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

            return _mapper.Map<List<TransactionDto>>(transactions);
        }
    }
}