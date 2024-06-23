using Microsoft.EntityFrameworkCore;
using ST10242546_CLDV6211_POE_.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid.Extensions.DependencyInjection;
using ST10242546_CLDV6211_POE_.Services;
using ST10242546_CLDV6211_POE_.Settings;

namespace ST10242546_CLDV6211_POE_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddDbContext<KhumaloCraftDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("KhumaloCraftDEV")));

            builder.Services.AddDbContext<KhumaloCraftDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("KhumaloCraftLIVE")));

            builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders().AddRoles<IdentityRole>().AddEntityFrameworkStores<KhumaloCraftDbContext>();
            builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));

            builder.Services.AddSendGrid(options =>
            {
                options.ApiKey = builder.Configuration.GetSection("SendGridSettings").GetValue<string>("ApiKey");
            });

            builder.Services.AddScoped<IEmailSender, EmailSenderService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
