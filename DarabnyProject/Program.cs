using DarabnyProject.Data;
using DarabnyProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DarabnyProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<DarabnyProject.Filters.CategoryNavFilter>();
            });
            builder.Services.AddScoped<DarabnyProject.Filters.CategoryNavFilter>();

            builder.Services.AddDbContext<DarabnyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ── Repositories ───────────────────────────────────────────
            builder.Services.AddScoped<DarabnyProject.Repositories.ICategoryRepository, DarabnyProject.Repositories.CategoryRepository>();
            builder.Services.AddScoped<DarabnyProject.Repositories.ICourseRepository, DarabnyProject.Repositories.CourseRepository>();
            builder.Services.AddScoped<DarabnyProject.Repositories.IChapterRepository, DarabnyProject.Repositories.ChapterRepository>();
            builder.Services.AddScoped<DarabnyProject.Repositories.ILessonRepository, DarabnyProject.Repositories.LessonRepository>();
            builder.Services.AddScoped<DarabnyProject.Repositories.IEnrollmentRepository, DarabnyProject.Repositories.EnrollmentRepository>();
            builder.Services.AddScoped<DarabnyProject.Repositories.IFavoriteRepository, DarabnyProject.Repositories.FavoriteRepository>();
            builder.Services.AddScoped<DarabnyProject.Repositories.IReviewRepository, DarabnyProject.Repositories.ReviewRepository>();
            builder.Services.AddScoped<DarabnyProject.Repositories.IProgressRepository, DarabnyProject.Repositories.ProgressRepository>();

            // ── Services ───────────────────────────────────────────────
            builder.Services.AddScoped<DarabnyProject.Services.ICategoryService, DarabnyProject.Services.CategoryService>();
            builder.Services.AddScoped<DarabnyProject.Services.ICourseService, DarabnyProject.Services.CourseService>();
            builder.Services.AddScoped<DarabnyProject.Services.IChapterService, DarabnyProject.Services.ChapterService>();
            builder.Services.AddScoped<DarabnyProject.Services.ILessonService, DarabnyProject.Services.LessonService>();
            builder.Services.AddScoped<DarabnyProject.Services.IEnrollmentService, DarabnyProject.Services.EnrollmentService>();
            builder.Services.AddScoped<DarabnyProject.Services.IFavoriteService, DarabnyProject.Services.FavoriteService>();
            builder.Services.AddScoped<DarabnyProject.Services.IReviewService, DarabnyProject.Services.ReviewService>();
            builder.Services.AddScoped<DarabnyProject.Services.IProgressService, DarabnyProject.Services.ProgressService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit           = true;
                options.Password.RequiredLength         = 6;
                options.Password.RequireUppercase       = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail         = true;
            })
            .AddEntityFrameworkStores<DarabnyDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath        = "/Account/Login";
                options.LogoutPath       = "/Account/Logout";
                options.AccessDeniedPath = "/Home/Index";
                // Cookie expires when browser closes (session cookie)
                options.ExpireTimeSpan    = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly   = true;
                // Session cookie — dies when browser closes
                options.Cookie.MaxAge     = null;
            });

            var app = builder.Build();

            // ── Seed Roles ─────────────────────────────────────────────
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles  = { "Admin", "Teacher", "Student" };
                foreach (var role in roles)
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
