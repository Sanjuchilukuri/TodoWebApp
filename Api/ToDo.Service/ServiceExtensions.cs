using ToDo.Service.Interfaces;
using ToDo.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ToDo.Service
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplication(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(ServiceExtensions));
            builder.Services.AddControllers().AddNewtonsoftJson();
        }
    }
}
