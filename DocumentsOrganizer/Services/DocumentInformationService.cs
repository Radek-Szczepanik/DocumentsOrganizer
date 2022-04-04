using AutoMapper;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Exceptions;
using DocumentsOrganizer.Models;
using System.Linq;

namespace DocumentsOrganizer.Services
{
    public class DocumentInformationService : IDocumentInformationService
    {
        private readonly DocumentsOrganizerDbContext context;
        private readonly IMapper mapper;

        public DocumentInformationService(DocumentsOrganizerDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public int CreateInformation(int documentId, CreateInformationDto dto)
        {
            var document = context.Documents.FirstOrDefault(d => d.Id == documentId);
            if (document is null)
            {
                throw new NotFoundException("Document not found");
            }

            var documentInformationEntity = mapper.Map<DocumentInformation>(dto);

            documentInformationEntity.DocumentId = documentId;

            context.Informations.Add(documentInformationEntity);
            context.SaveChanges();

            return documentInformationEntity.Id;
        }
    }
}
