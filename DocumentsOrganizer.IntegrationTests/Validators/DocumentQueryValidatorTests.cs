using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Models;
using DocumentsOrganizer.Models.Validators;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DocumentsOrganizer.IntegrationTests.Validators
{
    public class DocumentQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<DocumentQuery>()
            {
                new DocumentQuery()
                {
                    PageNumber = 1,
                    PageSize = 10
                },
                new DocumentQuery()
                {
                    PageNumber = 2,
                    PageSize = 15
                },
                new DocumentQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(Document.Name)
                },
                new DocumentQuery()
                {
                    PageNumber = 12,
                    PageSize = 15,
                },
            };

            return list.Select(q => new object[] { q });
        }


        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForCorrectModel_ReturnsSuccess(DocumentQuery model)
        {
            // arrange
            var validator = new DocumentQueryValidator();

            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<DocumentQuery>()
            {
                new DocumentQuery()
                {
                    PageNumber = 0,
                    PageSize = 10
                },
                new DocumentQuery()
                {
                    PageNumber = 2,
                    PageSize = 11
                },
                new DocumentQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(Document.CreatedBy)
                },
                new DocumentQuery()
                {
                    PageNumber = 12,
                    PageSize = 15,
                    SortBy = nameof(Document.Id)
                },
            };

            return list.Select(q => new object[] { q });
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForIncorrectModel_ReturnsFailure(DocumentQuery model)
        {
            // arrange
            var validator = new DocumentQueryValidator();

            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
