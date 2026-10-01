using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<User> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<string?> Login(LoginDto dto, CancellationToken ct)
        {
            var user = await _userManager.FindByNameAsync(dto.PhoneNumber);

            if (user == null)
                return null;

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
                return null;

            return _jwtService.GenerateToken(user.Id, user.PhoneNumber!);
        }

        public async Task<IEnumerable<string>> Register(RegisterDto dto, CancellationToken ct)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.PhoneNumber);

            if (existingUser != null)
            {
                return ["این شماره موبایل قبلاً ثبت شده است."];
            }

            var user = new User
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                UserName = dto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                return [];

            return result.Errors.Select(x => x.Description);
        }
    }
}
