using AutoMapper;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Exceptions;
using DocumentsOrganizer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DocumentsOrganizer.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentsOrganizerDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<DocumentService> logger;

        public DocumentService(DocumentsOrganizerDbContext dbContext, IMapper mapper, ILogger<DocumentService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IEnumerable<DocumentDto> GetAll()
        {
            var documents = dbContext
                .Documents
                .Include(i => i.DocumentInformations)
                .ToList();

            if (documents is null)
                throw new NotFoundException("Document not found");

            var result = mapper.Map<List<DocumentDto>>(documents);
            return result;
        }

        public DocumentDto GetById(int id)
        {
            var document = dbContext
                .Documents
                .Include(i => i.DocumentInformations)
                .FirstOrDefault(x => x.Id == id);

            if (document is null)
                throw new NotFoundException("Document not found");

            var result = mapper.Map<DocumentDto>(document);
            return result;
        }

        public int Create(CreateDocumentDto dto)
        {
            var document = mapper.Map<Document>(dto);
            dbContext.Documents.Add(document);
            dbContext.SaveChanges();

            return document.Id;
        }

        public void Update(int id, UpdateDocumentDto dto)
        {
            var document = dbContext.Documents.FirstOrDefault(x => x.Id == id);

            if (document is null)
                throw new NotFoundException("Document not found");

            document.Name = dto.Name;

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var document = dbContext.Documents.FirstOrDefault(x => x.Id == id);

            if(document is null)
                throw new NotFoundException("Document not found");

            dbContext.Documents.Remove(document);
            dbContext.SaveChanges();
        }
    }
}
