using AutoMapper;
using DocumentsOrganizer.Authorization;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Exceptions;
using DocumentsOrganizer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace DocumentsOrganizer.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentsOrganizerDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<DocumentService> logger;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public DocumentService(DocumentsOrganizerDbContext dbContext,
            IMapper mapper, 
            ILogger<DocumentService> logger,
            IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
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
            var document = mapper.Map<Entities.Document>(dto);
            document.CreatedById = userContextService.GetUserId;
            dbContext.Documents.Add(document);
            dbContext.SaveChanges();

            return document.Id;
        }

        public void Update(int id, UpdateDocumentDto dto)
        {
            var document = dbContext.Documents.FirstOrDefault(x => x.Id == id);

            if (document is null)
                throw new NotFoundException("Document not found");

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, document,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            document.Name = dto.Name;

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var document = dbContext.Documents.FirstOrDefault(x => x.Id == id);

            if(document is null)
                throw new NotFoundException("Document not found");

            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, document,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Documents.Remove(document);
            dbContext.SaveChanges();
        }
    }
}
