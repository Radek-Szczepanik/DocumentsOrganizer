using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Models;
using Microsoft.AspNetCore.Identity;

namespace DocumentsOrganizer.Services
{
    public class AccountService : IAccountService
    {
        private readonly DocumentsOrganizerDbContext context;
        private readonly IPasswordHasher<User> passwordHasher;

        public AccountService(DocumentsOrganizerDbContext context, IPasswordHasher<User> passwordHasher)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
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
    }
}
