using DocumentsOrganizer.Models;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentInformationService
    {
        int CreateInformation(int documentId, CreateInformationDto dto);
    }
}