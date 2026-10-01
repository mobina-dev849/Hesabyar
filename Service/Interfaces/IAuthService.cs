using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<IEnumerable<string>> Register(RegisterDto dto, CancellationToken ct);

        Task<string?> Login(LoginDto dto, CancellationToken ct);
    }
}
