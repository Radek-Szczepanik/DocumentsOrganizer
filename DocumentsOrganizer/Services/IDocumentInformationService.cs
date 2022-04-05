using DocumentsOrganizer.Models;
using System.Collections.Generic;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentInformationService
    {
        int CreateInformation(int documentId, CreateInformationDto dto);
        DocumentInformationDto GetById(int documentId, int documentInformationId);
        List<DocumentInformationDto> GetAll(int documentId);
    }
}