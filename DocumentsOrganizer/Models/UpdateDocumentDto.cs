using System.ComponentModel.DataAnnotations;

namespace DocumentsOrganizer.Models
{
    public class UpdateDocumentDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
