using DocumentsOrganizer.Models;
using System.Collections.Generic;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentService
    {
        int Create(CreateDocumentDto dto);
        IEnumerable<DocumentDto> GetAll();
        DocumentDto GetById(int id);
    }
}