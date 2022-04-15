using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Exceptions;
using DocumentsOrganizer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DocumentsOrganizer.Services
{
    public class AccountService : IAccountService
    {
        private readonly DocumentsOrganizerDbContext context;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;

        public AccountService(DocumentsOrganizerDbContext context,
                              IPasswordHasher<User> passwordHasher,
                              AuthenticationSettings authenticationSettings)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
        }

        public void ReqisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleId = dto.RoleId,
            };

            var hashedPassword = passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = context.Users
                .Include(r => r.Role)
                .FirstOrDefault(e => e.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.RoleName}"),    
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
