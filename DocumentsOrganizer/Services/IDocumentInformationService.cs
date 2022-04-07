using DocumentsOrganizer.Models;
using System.Collections.Generic;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentInformationService
    {
        List<DocumentInformationDto> GetAll(int documentId);
        DocumentInformationDto GetById(int documentId, int documentInformationId);
        int CreateInformation(int documentId, CreateInformationDto dto);
        void RemoveAll(int documentId);
    }
}