using System.Security.Claims;

namespace DocumentsOrganizer.Services
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}