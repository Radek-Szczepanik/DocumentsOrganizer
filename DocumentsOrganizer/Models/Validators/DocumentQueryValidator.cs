using FluentValidation;
using System.Linq;

namespace DocumentsOrganizer.Models.Validators
{
    public class DocumentQueryValidator : AbstractValidator<DocumentQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        public DocumentQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
                }
            });
        }
    }
}
