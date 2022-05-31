using AutoMapper;
using DocumentsOrganizer.Authorization;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Exceptions;
using DocumentsOrganizer.Models;
using Microsoft.AspNetCore.Authorization;
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

        private Document GetDocumentById(int documentId)
        {
            var document = context.Documents
                .Include(i => i.DocumentInformations)
                .FirstOrDefault(d => d.Id == documentId);
            if (document is null)
            {
                throw new NotFoundException("Information not found");
            }

            return document;
        }

        public List<DocumentInformationDto> GetAll(int documentId)
        {
            var document = GetDocumentById(documentId);
            var informationDto = mapper.Map<List<DocumentInformationDto>>(document.DocumentInformations);

            return informationDto;
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

        public int CreateInformation(int documentId, CreateInformationDto dto)
        {
            var document = GetDocumentById(documentId);
            var documentInformationEntity = mapper.Map<DocumentInformation>(dto);

            documentInformationEntity.DocumentId = documentId;

            context.Informations.Add(documentInformationEntity);
            context.SaveChanges();

            return documentInformationEntity.Id;
        }

        public void UpdateInformation(int documentId, int informationId, UpdateDocumentInformationDto dto)
        {
            var document = GetDocumentById(documentId);
            var information = context.Informations.FirstOrDefault(x => x.Id == informationId);

            if (document is null)
                throw new NotFoundException("Information not found");

            information.Description = dto.Description;

            context.SaveChanges();
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
    }
}