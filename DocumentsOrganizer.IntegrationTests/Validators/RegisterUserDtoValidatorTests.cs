using DocumentsOrganizer.Entities;
using DocumentsOrganizer.Models;
using DocumentsOrganizer.Models.Validators;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DocumentsOrganizer.IntegrationTests.Validators
{
    public class RegisterUserDtoValidatorTests
    {
        private DocumentsOrganizerDbContext dbContext;
        public RegisterUserDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<DocumentsOrganizerDbContext>();
            builder.UseInMemoryDatabase("TestDb");

            this.dbContext = new DocumentsOrganizerDbContext(builder.Options);
            Seed();
        }

        public void Seed()
        {
            var testUsers = new List<User>()
            {
                new User()
                {
                    Email = "test2@test.com"
                },
                new User()
                {
                    Email = "test3@test.com"
                }
            };

            this.dbContext.Users.AddRange(testUsers);
            this.dbContext.SaveChanges();
        }

        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<RegisterUserDto>()
            {
                new RegisterUserDto()
                {
                    Email = "test@test.com",
                    Password = "password123",
                    ConfirmPassword = "password123"
                },
                new RegisterUserDto()
                {
                    Email = "tes5@test.com",
                    Password = "password123",
                    ConfirmPassword = "password123"
                },
                new RegisterUserDto()
                {
                    Email = "test9@test.com",
                    Password = "password123",
                    ConfirmPassword = "password123"
                },
            };

            return list.Select(q => new object[] { q });
        }

        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_IfUserNotExist_ReturnsSuccess(RegisterUserDto registerUser)
        {
            // arrange
            var validator = new RegisterUserDtoValidator(this.dbContext);

            // act
            var result = validator.TestValidate(registerUser);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<RegisterUserDto>()
            {
                new RegisterUserDto()
                {
                    Email = "test2@test.com",
                    Password = "password123",
                    ConfirmPassword = "password123"
                },
                new RegisterUserDto()
                {
                    Email = "test3@test.com",
                    Password = "password123",
                    ConfirmPassword = "password123"
                },
                new RegisterUserDto()
                {
                    Email = "test12test.com",
                },
            };

            return list.Select(q => new object[] { q });
        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_IfUserExist_ReturnsFailure(RegisterUserDto registerUser)
        {
            // arrange
            var validator = new RegisterUserDtoValidator(this.dbContext);

            // act
            var result = validator.TestValidate(registerUser);

            // assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
