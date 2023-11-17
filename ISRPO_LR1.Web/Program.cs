using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Repositories;
using ISRPO_LR1.Web.Repositories.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<BaseRepository<Grade>, GradesRepository>();
builder.Services.AddScoped<BaseRepository<Student>, StudentsRepository>();
builder.Services.AddScoped<BaseRepository<Subject>, SubjectsRepository>();

var apiWebAddress = builder.Configuration.GetValue<string>("ApiWebAddress", "http://localhost:5000");
builder.Services.AddScoped<HttpClient>(_ =>
    new HttpClient()
    {
        BaseAddress = new Uri(apiWebAddress ?? string.Empty)
    });

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();