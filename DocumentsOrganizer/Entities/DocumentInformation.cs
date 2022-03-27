namespace DocumentsOrganizer.Entities
{
    public class DocumentInformation : EntityBase
    {
        public string Description { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; }
    }
}