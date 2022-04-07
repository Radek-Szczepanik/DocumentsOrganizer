using AutoMapper;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Exceptions;
using DocumentsOrganizer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            var document = GetDocumentById(documentId);
            var documentInformationEntity = mapper.Map<DocumentInformation>(dto);

            documentInformationEntity.DocumentId = documentId;

            context.Informations.Add(documentInformationEntity);
            context.SaveChanges();

            return documentInformationEntity.Id;
        }

        public DocumentInformationDto GetById(int documentId, int documentInformationId)
        {
            var document = GetDocumentById(documentId);
            var information = context.Informations.FirstOrDefault(i => i.Id == documentInformationId);

            if (information is null || information.Id != documentInformationId)
            {
                throw new NotFoundException("Information not found");
            }

            var informationDto = mapper.Map<DocumentInformationDto>(information);

            return informationDto;
        }

        public List<DocumentInformationDto> GetAll(int documentId)
        {
            var document = GetDocumentById(documentId);
            var informationDto = mapper.Map<List<DocumentInformationDto>>(document.DocumentInformations);

            return informationDto;
        }

        public void RemoveAll(int documentId)
        {
            var document = GetDocumentById(documentId);

            context.RemoveRange(document.DocumentInformations);
            context.SaveChanges();
        }

        public void RemoveById(int documentId, int documentInformationId)
        {
            var document = GetDocumentById(documentId);
            var information = context.Informations.FirstOrDefault(i => i.Id == documentInformationId);

            if (information is null || information.Id != documentInformationId)
            {
                throw new NotFoundException("Information not found");
            }

            context.Remove(information);
            context.SaveChanges();
        }

        private Document GetDocumentById(int documentId)
        {
            var document = context.Documents
                .Include(i => i.DocumentInformations)
                .FirstOrDefault(d => d.Id == documentId);
            if (document is null)
            {
                throw new NotFoundException("Document not found");
            }

            return document;
        }
    }
}
