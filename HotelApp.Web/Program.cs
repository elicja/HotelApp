using HotelAppLibrary.Data.EF;
using HotelAppLibrary.Data.Sql;
using HotelAppLibrary.Data.Sqllite;
using HotelAppLibrary.Interfaces;

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
else if (dbChoice == "ef")
{

    builder.Services.AddTransient<IDatabaseData, EFDataAccess>();
}
else
{
    // Default
    builder.Services.AddTransient<IDatabaseData, SqlData>();
}

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();
builder.Services.AddTransient<IEFDataAccess, EFDataAccess>();

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
