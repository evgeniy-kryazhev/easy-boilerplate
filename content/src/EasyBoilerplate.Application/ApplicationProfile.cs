using AutoMapper;
using EasyBoilerplate.Application.Services.Identity;
using EasyBoilerplate.Domain.Identity;

namespace EasyBoilerplate.Application;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<ApplicationUser, ApplicationUserDto>();
    }
}