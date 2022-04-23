using DocumentsOrganizer.Models;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentService
    {
        PageResult<DocumentDto> GetAll(DocumentQuery query);
        DocumentDto GetById(int id);
        int Create(CreateDocumentDto dto);
        void Update(int id, UpdateDocumentDto dto);
        void Delete(int id);
    }
}