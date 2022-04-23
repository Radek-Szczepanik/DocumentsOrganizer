using DocumentsOrganizer.Models;
using System.Collections.Generic;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentService
    {
        IEnumerable<DocumentDto> GetAll(string searchPhrase);
        DocumentDto GetById(int id);
        int Create(CreateDocumentDto dto);
        void Update(int id, UpdateDocumentDto dto);
        void Delete(int id);
    }
}