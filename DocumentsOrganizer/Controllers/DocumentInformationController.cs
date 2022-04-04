using DocumentsOrganizer.Models;
using DocumentsOrganizer.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public ActionResult CreateDocumentInformation([FromRoute] int documentId, [FromBody] CreateInformationDto dto)
        {
            var informationId = documentInformationService.CreateInformation(documentId, dto);
            return Created($"api/{documentId}/information/{informationId}", null);
        }
    }
}
