using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using My_Core_Project.DBContext;
using My_Core_Project.Helpers;
using My_Core_Project.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // For MVC controllers and views
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());// Register AutoMapper with the MappingProfile

builder.Services.AddTransient<IEmployee,EmployeeRepo>();
builder.Services.AddTransient<IUser, UserRepo>();
builder.Services.AddTransient<IDepartment, DepartmentRepo>();
builder.Services.AddTransient<IDesignation, DesignationRepo>();
builder.Services.AddTransient<IAttendance, AttendanceRepo>();
builder.Services.AddTransient<ILeaveRequest, LeaveRequestRepo>();
builder.Services.AddTransient<ISalary, SalaryRepo>();


var connectionstring = builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddDbContext<HrmsDbContext>(x=>x.UseSqlServer(connectionstring));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Login/Index"; // Redirect to this path if not authenticated
        option.LogoutPath = "/Login/Index"; //   Redirect to this path when logging out
        option.AccessDeniedPath = "/Login/AccessDenied";// Redirect to this path if access is denied
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())                          /*--> for development environment error handling*/
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseExceptionHandler("/Home/Error");              // ---> for production and devlopment {both} environment error handling
//if (!app.Environment.IsDevelopment())
//{
//    app.UseHsts();
//}


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
