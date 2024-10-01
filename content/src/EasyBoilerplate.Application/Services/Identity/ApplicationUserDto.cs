namespace EasyBoilerplate.Application.Services.Identity;

public class ApplicationUserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
}