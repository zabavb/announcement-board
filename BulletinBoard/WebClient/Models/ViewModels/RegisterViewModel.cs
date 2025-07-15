using System.ComponentModel.DataAnnotations;

namespace WebClient.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one letter and one number.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}