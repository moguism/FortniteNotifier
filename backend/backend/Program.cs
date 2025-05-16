using backend.Models.Mappers;
using backend.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Microsoft.Playwright.Program.Main(["install"]);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<Context>();
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<UserMapper>();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<WishlistService>();
            builder.Services.AddScoped<PlaywrightService>();
            builder.Services.AddScoped<EmailService>();

            builder.Services.AddHostedService<RefreshShopBackgroundService>();
            builder.Services.AddHostedService<HeartBeatBackgroundService>();

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            // CONFIGURANDO JWT
            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    string key = Environment.GetEnvironmentVariable("JwtKey");
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        // INDICAMOS LA CLAVE
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            SeedDataBaseAsync(app.Services);

            app.Run();
        }

        static void SeedDataBaseAsync(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            using Context dbContext = scope.ServiceProvider.GetService<Context>();

            dbContext.Database.EnsureCreated();
        }
    }
}
