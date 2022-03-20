using System.Collections.Generic;

namespace DocumentsOrganizer.Entities
{
    public class Document : EntityBase
    {
        public string Name { get; set; }

        public virtual List<DocumentInformation> Informations { get; set; }
    }
}
