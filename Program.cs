using Microsoft.EntityFrameworkCore;
using SRMS.Data;
using SRMS.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StudentRecordDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDB")));
builder.Services.AddConfig(builder.Configuration).AddConfigDependencyGroup();
builder.Services.AddDataDependencyGroup();
builder.Services.AddUtilsDependencyGroup();
builder.Services.AddSession();

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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
