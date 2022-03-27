using DocumentsOrganizer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DocumentsOrganizer
{
    public class DocumentsOrganizerSeeder
    {
        private readonly DocumentsOrganizerDbContext dbContext;

        public DocumentsOrganizerSeeder(DocumentsOrganizerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Seed()
        {
            if (this.dbContext.Database.CanConnect())
            {
                if (!dbContext.Documents.Any())
                {
                    var documents = GetDocuments();
                    this.dbContext.AddRange(documents);
                    this.dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Document> GetDocuments()
        {
            var documents = new List<Document>()
            {
                new Document()
                {
                    Name = "Orange",
                    DocumentInformations = new List<DocumentInformation>()
                    {
                        new DocumentInformation()
                        {
                            Description = "Umowa z Orange podpisana w dniu ...."
                        },
                        new DocumentInformation()
                        {
                            Description = "Wypowiedzieć umowę do dnia ..."
                        }
                    }
                }  
            };

            return documents;
        }
    }
}
