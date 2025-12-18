using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // --- 1️⃣ Seed books only if none exist ---
    if (!context.Books.Any())
    {
        context.Books.AddRange(
            new Book
            {
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Price = 25,
                Stock = 30,
                Edition = "1st",
                CoverImageUrl = "https://example.com/clean-code.jpg",
                CategoryId = 1, // IT
                PopularityScore = 0
            },
            new Book
            {
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt, David Thomas",
                Price = 30,
                Stock = 20,
                Edition = "2nd",
                CoverImageUrl = "https://example.com/pragmatic.jpg",
                CategoryId = 1, // IT
                PopularityScore = 0
            },
            new Book
            {
                Title = "Sapiens: A Brief History of Humankind",
                Author = "Yuval Noah Harari",
                Price = 18,
                Stock = 40,
                Edition = "1st",
                CoverImageUrl = "https://example.com/sapiens.jpg",
                CategoryId = 2, // History
                PopularityScore = 0
            },
            new Book
            {
                Title = "Guns, Germs, and Steel",
                Author = "Jared Diamond",
                Price = 20,
                Stock = 25,
                Edition = "1st",
                CoverImageUrl = "https://example.com/guns.jpg",
                CategoryId = 2, // History
                PopularityScore = 0
            },
            new Book
            {
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                Price = 15,
                Stock = 50,
                Edition = "3rd",
                CoverImageUrl = "https://example.com/pride.jpg",
                CategoryId = 3, // Classics
                PopularityScore = 0
            },
            new Book
            {
                Title = "Moby Dick",
                Author = "Herman Melville",
                Price = 17,
                Stock = 35,
                Edition = "2nd",
                CoverImageUrl = "https://example.com/mobydick.jpg",
                CategoryId = 3, // Classics
                PopularityScore = 0
            }
        );

        context.SaveChanges();
    }
}





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


















