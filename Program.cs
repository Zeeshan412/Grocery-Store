// Microsoft Framework
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Application Data Layer
using WebApplication4.DataLayer.DbContext;
using WebApplication4.DataLayer.Initializers;

// Application Entities
using WebApplication4.Entities;

// Application Business Logic
using WebApplication4.BusinessLogic.Factories;
using WebApplication4.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<UpdatedUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found");
var issuer = jwtSettings["Issuer"] ?? "Grossbox";
var audience = jwtSettings["Audience"] ?? "GrossboxUsers";

// JWT Authentication Configuration
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
// Cookie authentication is already configured by AddDefaultIdentity

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy =>policy.RequireAuthenticatedUser());
    options.AddPolicy("AdminAccess", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
    options.AddPolicy("CustomerAccess", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Customer"));
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o=> o.IdleTimeout = TimeSpan.FromMinutes(2));

// Register Business Logic Layer Services
builder.Services.AddScoped<IOrderService>(provider => new OrderService(connectionString));
builder.Services.AddScoped<IProductService>(provider => new ProductService(connectionString));
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService>(provider => new CategoryService(connectionString));
builder.Services.AddScoped<RepositoryFactory>(provider => new RepositoryFactory(connectionString));
builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
var app = builder.Build();

// Initialize database tables (Product, Category, Order, OrderProducts)
DatabaseInitializer.InitializeDatabase(connectionString);

// Seed sample data for testing (only if database is empty)
SampleDataSeeder.SeedSampleData(connectionString);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
