using System.ComponentModel.DataAnnotations;

namespace EasyBoilerplate.Host.ViewModels;

public class RegisterViewModel
{
    [Required] public string Email { get; init; } = default!;
    [Required] public string Password { get; init; } = default!;
}