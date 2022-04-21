using DocumentsOrganizer.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocumentsOrganizer.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Document>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceOperationRequirement requirement,
            Document document)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
               requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (document.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
