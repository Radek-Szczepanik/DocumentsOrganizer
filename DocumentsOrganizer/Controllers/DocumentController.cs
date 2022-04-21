using DocumentsOrganizer.Models;
using DocumentsOrganizer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace DocumentsOrganizer.Controllers
{
    [Route("api/document")]
    [ApiController]
    [Authorize]
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
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var documentId = documentService.Create(dto, userId);

            return Created($"api/document/{documentId}", null);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateDocument([FromBody] UpdateDocumentDto dto, [FromRoute] int id)
        {
            documentService.Update(id, dto, User);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDocument([FromRoute] int id)
        {
            documentService.Delete(id, User);

            return NoContent();
        }
    }
}
