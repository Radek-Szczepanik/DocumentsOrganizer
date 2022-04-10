using System.ComponentModel.DataAnnotations;

namespace DocumentsOrganizer.Models
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
