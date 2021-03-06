using System.Collections.Generic;

namespace DocumentsOrganizer.Entities
{
    public class Document : EntityBase
    {
        public string Name { get; set; }
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public virtual List<DocumentInformation> DocumentInformations { get; set; }
    }
}
