using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

string dbChoice = builder.Configuration.GetValue<string>("DatabaseChoice").ToLower();
if (dbChoice == "sql")
{
    builder.Services.AddTransient<IDatabaseData, SqlData>();
}
else if (dbChoice == "sqlite")
{
    builder.Services.AddTransient<IDatabaseData, SqliteData>();
}
else
{
    // Default
    builder.Services.AddTransient<IDatabaseData, SqlData>();
}

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
