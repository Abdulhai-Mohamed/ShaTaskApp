using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShaTaskApp.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ShaTaskDBConnection") ?? throw new InvalidOperationException("Connection string 'ShaTaskDBConnection' not found.");

//create serviceProvider instance
ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();

//create IConfiguration instance
IConfiguration iConfiguration = serviceProvider.GetRequiredService<IConfiguration>();

//create IWebHostEnvironment instance
IWebHostEnvironment iWebHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

//create IServiceCollection instance
IServiceCollection iServiceCollection = builder.Services;

//services

// Add services to the container.
iServiceCollection.AddControllersWithViews();
//Register  MyDbContext service in DbContext class in DI
iServiceCollection.AddDbContextPool<ShaTaskDB>(
            dbContextOptionsBuilder =>
              dbContextOptionsBuilder.UseSqlServer(iConfiguration.GetConnectionString("ShaTaskDBConnection")));

iServiceCollection.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ShaTaskDB>()
        .AddDefaultTokenProviders();

//*****
var app = builder.Build();
//Middlewares

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Business}/{action=InvoicesList}/{id?}");

app.Run();