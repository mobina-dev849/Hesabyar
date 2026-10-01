using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(long userId, string phoneNumber);
    }
}
