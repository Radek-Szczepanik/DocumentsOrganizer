using System.ComponentModel.DataAnnotations;

namespace DocumentsOrganizer.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
