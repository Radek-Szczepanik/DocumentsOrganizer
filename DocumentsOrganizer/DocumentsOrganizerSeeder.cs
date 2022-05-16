using DocumentsOrganizer.Entities;
using Microsoft.EntityFrameworkCore;
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
            if (dbContext.Database.CanConnect())
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                }

                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    dbContext.SaveChanges();
                }

                if (!dbContext.Documents.Any())
                {
                    var documents = GetDocuments();
                    dbContext.Documents.AddRange(documents);
                    dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    RoleName = "User"
                },
                new Role()
                {
                    RoleName = "Owner"
                },
                new Role()
                {
                    RoleName = "Admin"
                }
            };

            return roles;
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
