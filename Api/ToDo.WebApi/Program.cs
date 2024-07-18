using ToDo.Service;
using ToDo.Data;
using ToDo.WebApi.Middleware;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ToDo.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                         .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                         .CreateLogger();

            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.ConfigureApplication();

            builder.ConfigureInfrastructure(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "ToDOApp Api",
                    Version = "V1",
                    Description = "A Breif description about API",
                    TermsOfService = new Uri("https://sanju.org/privacy-policy"),
                    Contact = new OpenApiContact
                    {
                        Name = "Support",
                        Email = "support@sanju.org.net",
                        Url = new Uri("https://sanju.org/contact/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use Under XYZ",
                        Url = new Uri("https://sanju.org/about-us/")
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = $"JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                                }
                        },
                        new string[]{}
                    }
                });

            });

            var apiCorsPolicy = "ApiCorsPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: apiCorsPolicy,
                            builder =>
                            {
                                builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                            });
            });

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "My API V1");
            });

            app.UseCors(apiCorsPolicy);

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalErrorHandling>();

            app.UseMiddleware<Logging>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}