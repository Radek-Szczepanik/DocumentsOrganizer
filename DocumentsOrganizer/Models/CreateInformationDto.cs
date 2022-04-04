using System.ComponentModel.DataAnnotations;

namespace DocumentsOrganizer.Models
{
    public class CreateInformationDto
    {
        [Required]
        public string Description { get; set; }
        public int DocumentId { get; set; }
    }
}
