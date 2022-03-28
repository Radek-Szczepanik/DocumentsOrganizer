using AutoMapper;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DocumentsOrganizer.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentsOrganizerDbContext dbContext;
        private readonly IMapper mapper;

        public DocumentService(DocumentsOrganizerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IEnumerable<DocumentDto> GetAll()
        {
            var documents = dbContext
                .Documents
                .Include(i => i.DocumentInformations)
                .ToList();

            if (documents is null) return null;

            var result = mapper.Map<List<DocumentDto>>(documents);
            return result;
        }

        public DocumentDto GetById(int id)
        {
            var document = dbContext
                .Documents
                .Include(i => i.DocumentInformations)
                .FirstOrDefault(x => x.Id == id);

            if (document is null) return null;

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

        public bool Update(int id, UpdateDocumentDto dto)
        {
            var document = dbContext.Documents.FirstOrDefault(x => x.Id == id);

            if (document is null)
            {
                return false;
            }

            document.Name = dto.Name;

            dbContext.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var document = dbContext.Documents.FirstOrDefault(x => x.Id == id);

            if(document is null) return false;

            dbContext.Documents.Remove(document);
            dbContext.SaveChanges();

            return true;
        }
    }
}
