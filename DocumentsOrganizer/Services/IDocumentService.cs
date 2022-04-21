using DocumentsOrganizer.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentService
    {
        IEnumerable<DocumentDto> GetAll();
        DocumentDto GetById(int id);
        int Create(CreateDocumentDto dto, int userId);
        void Update(int id, UpdateDocumentDto dto, ClaimsPrincipal user);
        void Delete(int id, ClaimsPrincipal user);
    }
}