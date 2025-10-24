using APIExplorerApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

// Register your custom services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IDogService, DogService>();

var app = builder.Build();


var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
Console.WriteLine($"wwwroot path: {webRootPath}");
Console.WriteLine($"wwwroot exists: {Directory.Exists(webRootPath)}");

if (Directory.Exists(webRootPath))
{
    Console.WriteLine("Files in wwwroot:");
    foreach (var file in Directory.GetFiles(webRootPath, "*", SearchOption.AllDirectories))
    {
        Console.WriteLine($"  - {file}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();