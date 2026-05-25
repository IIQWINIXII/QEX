using Microsoft.AspNetCore.Mvc;
using QEX_Server.Dtos;
using QEX_Lib.ClientDB.Model;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// --- In-memory "БД" ---
var users = new List<User>();
var extensions = new List<Extension>();

// Для теста добавим одно расширение
extensions.Add(new Extension
{
    ID = "qex.writer",
    Name = "Writer",
    Description = "Simple text editor extension",
    Author = "System",
    Versions =
    {
        new ExtensionVersion
        {
            Version = "0.1.0",
            ManifestJson = """
            {
              "Id": "qex.writer",
              "Name": "Writer",
              "Version": "0.1.0",
              "AssemblyFile": "Writer.dll",
              "RootComponent": "Writer.Pages.WriterHome"
            }
            """,
            PackageUrl = "https://example.com/packages/qex.writer-0.1.0.zip"
        }
    }
});

// --- Auth: регистрация ---
app.MapPost("/api/auth/register", ([FromBody] RegisterRequest req) =>
{
    if (users.Any(u => u.UserName == req.UserName))
        return Results.BadRequest(new AuthResponse(false, "User already exists", null));

    var user = new User
    {
        UserName = req.UserName,
        Password = req.Password // потом заменишь на hash
    };
    users.Add(user);

    // пока токен — просто base64 от username
    var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.UserName));
    return Results.Ok(new AuthResponse(true, "Registered", token));
});

// --- Auth: логин ---
app.MapPost("/api/auth/login", ([FromBody] LoginRequest req) =>
{
    var user = users.FirstOrDefault(u => u.UserName == req.UserName && u.Password == req.Password);
    if (user is null)
        return Results.Unauthorized();

    var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.UserName));
    return Results.Ok(new AuthResponse(true, "OK", token));
});

// --- Список расширений ---
app.MapGet("/api/extensions", () =>
{
    var list = extensions.Select(e =>
    {
        var latest = e.Versions.OrderByDescending(v => v.PublishedAt).FirstOrDefault();
        return new ExtensionListItemDto(
            e.ExtensionId,
            e.Name,
            e.Description,
            e.Author,
            latest?.Version ?? "0.0.0"
        );
    });

    return Results.Ok(list);
});

// --- Детали расширения ---
app.MapGet("/api/extensions/{extensionId}", (string extensionId) =>
{
    var ext = extensions.FirstOrDefault(e => e.ExtensionId == extensionId);
    if (ext is null) return Results.NotFound();

    var latest = ext.Versions.OrderByDescending(v => v.PublishedAt).First();
    var dto = new ExtensionDetailsDto(
        ext.ExtensionId,
        ext.Name,
        ext.Description,
        ext.Author,
        latest.Version,
        latest.ManifestJson,
        latest.PackageUrl
    );

    return Results.Ok(dto);
});

// --- Публикация/обновление расширения ---
app.MapPost("/api/extensions/publish", ([FromBody] PublishExtensionRequest req) =>
{
    var ext = extensions.FirstOrDefault(e => e.ExtensionId == req.ExtensionId);
    if (ext is null)
    {
        ext = new Extension
        {
            ExtensionId = req.ExtensionId,
            Name = req.Name,
            Description = req.Description,
            Author = req.Author
        };
        extensions.Add(ext);
    }
    else
    {
        ext.Name = req.Name;
        ext.Description = req.Description;
        ext.Author = req.Author;
    }

    ext.Versions.Add(new ExtensionVersion
    {
        Version = req.Version,
        ManifestJson = req.ManifestJson,
        PackageUrl = req.PackageUrl
    });

    return Results.Ok();
});

// --- "Установка" расширения (сервер просто подтверждает) ---
app.MapPost("/api/extensions/install", ([FromBody] InstallRequest req) =>
{
    var ext = extensions.FirstOrDefault(e => e.ExtensionId == req.ExtensionId);
    if (ext is null)
        return Results.NotFound(new InstallResponse(false, "Extension not found"));

    var version = ext.Versions.FirstOrDefault(v => v.Version == req.Version)
                  ?? ext.Versions.OrderByDescending(v => v.PublishedAt).First();

    return Results.Ok(new
    {
        Success = true,
        Message = "Install confirmed",
        ExtensionId = ext.ExtensionId,
        Version = version.Version,
        PackageUrl = version.PackageUrl,
        ManifestJson = version.ManifestJson
    });
});

// --- Проверка обновлений ---
app.MapGet("/api/extensions/{extensionId}/updates", (string extensionId, string currentVersion) =>
{
    var ext = extensions.FirstOrDefault(e => e.ExtensionId == extensionId);
    if (ext is null) return Results.NotFound();

    // очень тупое сравнение версий, потом можно заменить на нормальное
    var newer = ext.Versions
        .Where(v => string.Compare(v.Version, currentVersion, StringComparison.OrdinalIgnoreCase) > 0)
        .OrderByDescending(v => v.PublishedAt)
        .FirstOrDefault();

    if (newer is null)
        return Results.Ok(null);

    var dto = new ExtensionDetailsDto(
        ext.ExtensionId,
        ext.Name,
        ext.Description,
        ext.Author,
        newer.Version,
        newer.ManifestJson,
        newer.PackageUrl
    );

    return Results.Ok(dto);
});

app.Run();
