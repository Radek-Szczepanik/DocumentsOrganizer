using DocumentsOrganizer.Models;

namespace DocumentsOrganizer.Services
{
    public interface IAccountService
    {
        void ReqisterUser(RegisterUserDto dto);
    }
}
