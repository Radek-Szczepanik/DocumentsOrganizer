﻿using DocumentsOrganizer.Models;
using System.Collections.Generic;

namespace DocumentsOrganizer.Services
{
    public interface IDocumentService
    {
        IEnumerable<DocumentDto> GetAll();
        DocumentDto GetById(int id);
        int Create(CreateDocumentDto dto);
        public bool Delete(int id);
    }
}