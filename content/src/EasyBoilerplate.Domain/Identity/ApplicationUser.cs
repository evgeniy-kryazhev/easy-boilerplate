using EasyBoilerplate.Shared;
using Microsoft.AspNetCore.Identity;

namespace EasyBoilerplate.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? CompanyId { get; set; }
}