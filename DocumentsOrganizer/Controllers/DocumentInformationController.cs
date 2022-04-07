using DocumentsOrganizer.Models;
using DocumentsOrganizer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DocumentsOrganizer.Controllers
{
    [Route("api/document/{documentId}/information")]
    [ApiController]
    public class DocumentInformationController : ControllerBase
    {
        private readonly IDocumentInformationService documentInformationService;

        public DocumentInformationController(IDocumentInformationService documentInformationService)
        {
            this.documentInformationService = documentInformationService;
        }

        [HttpGet]
        public ActionResult<List<DocumentInformationDto>> GetAll([FromRoute] int documentId)
        {
            var result = documentInformationService.GetAll(documentId);
            return Ok(result);
        }

        [HttpGet]
        [Route("{documentInformationId}")]
        public ActionResult<DocumentInformationDto> GetById([FromRoute] int documentId, [FromRoute] int documentInformationId)
        {
            DocumentInformationDto information = documentInformationService.GetById(documentId, documentInformationId);
            return Ok(information);
        }

        [HttpPost]
        public ActionResult CreateDocumentInformation([FromRoute] int documentId, [FromBody] CreateInformationDto dto)
        {
            var informationId = documentInformationService.CreateInformation(documentId, dto);
            return Created($"api/document/{documentId}/information/{informationId}", null);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int documentId)
        {
            documentInformationService.RemoveAll(documentId);

            return NoContent();
        }
    }
}
