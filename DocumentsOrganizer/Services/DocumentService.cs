using AutoMapper;
using DocumentsOrganizer.Authorization;
using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Exceptions;
using DocumentsOrganizer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public PageResult<DocumentDto> GetAll(DocumentQuery query)
        {
            var baseQuery = dbContext
                .Documents
                .Include(i => i.DocumentInformations)
                .Where(s => query.SearchPhrase == null || s.Name.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Document, object>>>
                {
                    { nameof(Document.Name), r => r.Name }
                };

                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var documents = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            if (documents is null)
                throw new NotFoundException("Document not found");

            var documentsDto = mapper.Map<List<DocumentDto>>(documents);

            var result = new PageResult<DocumentDto>(documentsDto, totalItemsCount, query.PageSize, query.PageNumber);

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
