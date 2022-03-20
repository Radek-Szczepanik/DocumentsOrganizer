namespace DocumentsOrganizer.Entities
{
    public class DocumentInformation : EntityBase
    {
        public string Description { get; set; }

        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }
}