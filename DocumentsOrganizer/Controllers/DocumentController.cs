using DocumentsOrganizer.Models;
using DocumentsOrganizer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DocumentsOrganizer.Controllers
{
    [Route("api/document")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;

        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DocumentDto>> GetAll()
        {
            var documents = documentService.GetAll();
            return Ok(documents);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<DocumentDto> GetById([FromRoute] int id)
        {
            var document = documentService.GetById(id);
            return Ok(document);
        }

        [HttpPost]
        public ActionResult CreateDocument([FromBody] CreateDocumentDto dto)
        {
            var documentId = documentService.Create(dto);
            return Created($"api/document/{documentId}", null);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDocument([FromRoute] int id)
        {
            var isDeleted = documentService.Delete(id);
            
            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
