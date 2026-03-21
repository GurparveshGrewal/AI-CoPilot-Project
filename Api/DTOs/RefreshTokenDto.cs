namespace Api.DTOs;

using System.ComponentModel.DataAnnotations;

public class RefreshTokenDto
{
    [Required]
    [MinLength(10)]
    public string RefreshToken { get; set; }
}
