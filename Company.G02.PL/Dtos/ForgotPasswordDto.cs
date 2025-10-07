using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email format!")]
        public string Email { get; set; }
    }
}
