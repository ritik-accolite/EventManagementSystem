using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplicationServer.Data;
using WebApplicationServer.Models;
using WebApplicationServer.Services;
using WebApplicationServer.Services.IService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddAuthorization();


// Register CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.AllowAnyOrigin();
            //builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});
// for testing , sql 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Create a SqlConnection object using the connection string
//using var conn = new SqlConnection(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
//conn.Open(); // Open the connection
// Configure DbContextOptionsBuilder to use the SqlConnection
//builder.Services.AddDbContext<DbContext>(options =>
//{
//    options.UseSqlServer(conn);
//});

builder.Services.AddIdentityApiEndpoints<Person>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IAddEventService, AddEventService>();
builder.Services.AddScoped<IAddBookedEventService, AddBookedEventService>();
builder.Services.AddScoped<IGetAllPerson,GetAllPerson>();


builder.Services.AddIdentityCore<Person>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;

})
.AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapIdentityApi<Person>();


// Uncomment this for access through 1 origin only
app.UseCors("AllowOrigin");
app.UseRouting();
app.MapControllerRoute(
    name: default,
    pattern: "{controller=Person}/{action = GetPerson}");

app.Run();


