using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDo.Data.DBContext;
using ToDo.Data.Interfaces;
using ToDo.Data.Repo;

namespace ToDo.Data
{
    public static class ServiceExtensions
    {
        public static void ConfigureInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IToDoItemRepo, ToDoItemRepo>();
            builder.Services.AddDbContext<ToDoDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DBConeection")));
        }
    }
}
