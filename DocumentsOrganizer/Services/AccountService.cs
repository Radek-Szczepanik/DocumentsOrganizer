using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Models;

namespace DocumentsOrganizer.Services
{
    public class AccountService : IAccountService
    {
        private readonly DocumentsOrganizerDbContext context;

        public AccountService(DocumentsOrganizerDbContext context)
        {
            this.context = context;
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

            context.Users.Add(newUser);
            context.SaveChanges();
        }
    }
}
