using Microsoft.EntityFrameworkCore;
using MvcCubosCarritoJuernes.Data;
using MvcCubosCarritoJuernes.Helper;
using MvcCubosCarritoJuernes.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
string connectionString = builder.Configuration.GetConnectionString("MySqlCubos");
builder.Services.AddTransient<RepositoryCubos>();
builder.Services.AddDbContext<CubosContext>
    (options => options.UseMySQL(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
