using System.Collections.Generic;

namespace DocumentsOrganizer.Models
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DocumentInformationDto> Informations { get; set; }
    }
}
