using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Models;
using PersonalAccount.Models.Students;
using PersonalAccount.Repository;
using PersonalAccount.Repository.Mappers;
using PersonalAccount.Services;
using PersonalAccount.Services.Auth;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Services.Db;

namespace PersonalAccount
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    // options.AccessDeniedPath = "/Account/AccessDenied";
                });
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDefaultConnection")));

            builder.Services.AddScoped<IStudentAuthService, StudentAuthService>();
            builder.Services.AddScoped<IStudentCabinetService, StudentCabinetService>();
            builder.Services.AddScoped<IConfirmationTokenService, ConfirmationTokenTokenService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();

            builder.Services.AddScoped<IStudentRepo<StudentAuthModel>, StudentRepo<StudentAuthModel>>();
            builder.Services.AddScoped<IStudentRepo<StudentModel>, StudentRepo<StudentModel>>();
            builder.Services.AddScoped<IConfirmationTokenRepo, ConfirmationTokenRepo>();
            
            builder.Services.AddScoped<IMapper<StudentEntity, StudentAuthModel>, StudentAuthMapper>();
            builder.Services.AddScoped<IMapper<StudentEntity, StudentModel>, StudentMapper>();
            builder.Services.AddScoped<IMapper<ConfirmationTokenEntity, ConfirmationTokenModel>, ConfirmationTokenMapper>();

            builder.Services.AddScoped<IPasswordHasher<StudentAuthModel>, PasswordHasher<StudentAuthModel>>();
            if (builder.Environment.IsDevelopment())
                builder.Services.AddScoped<DbSeeder>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
               using var scope = app.Services.CreateScope();
               var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
               await seeder.SeedAsync();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
// Razor Pages
// Client -> Server -> Page -> Model -> On[GET/POST]
//                             Model -> Update(Page) -> Client
//      Page > Model

// MVC
// Client -> Server -> Controller -> [Model]
//                     Controller -> View -> Client
//      Controller -> Client
//      |       |
//      View    Model

// Web-API
// Client -> Backend -> Controller -> Services -> Repository
//                      Controller -> [JSON] -> Client
// Client -> Frontend -> HTML, CSS, JS

// MVVM Model View View-Model