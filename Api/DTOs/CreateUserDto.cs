namespace Api.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}
