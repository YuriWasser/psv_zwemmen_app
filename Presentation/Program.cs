using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Repositories;
using Core.Interface;
using Core.Service;

var builder = WebApplication.CreateBuilder(args);

// ✅ Razor Pages toevoegen
builder.Services.AddRazorPages();

// ✅ Configuration beschikbaar maken voor JWT Settings
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// ✅ Connection string ophalen
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TrainOnly", policy =>
        policy.RequireRole("Train"));
});

// ✅ Repositories met connection string en logging
builder.Services.AddScoped<ICompetitieRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<CompetitieRepository>>();
    return new CompetitieRepository(connectionString, logger);
});

builder.Services.AddScoped<IProgrammaRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<ProgrammaRepository>>();
    return new ProgrammaRepository(connectionString, logger);
});

builder.Services.AddScoped<IAfstandRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<AfstandRepository>>();
    return new AfstandRepository(connectionString, logger);
});

builder.Services.AddScoped<IZwembadRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<ZwembadRepository>>();
    return new ZwembadRepository(connectionString, logger);
});

builder.Services.AddScoped<IGebruikerRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<GebruikerRepository>>();
    return new GebruikerRepository(connectionString, logger);
});

builder.Services.AddScoped<IFunctieRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<FunctieRepository>>();
    return new FunctieRepository(connectionString, logger);
});

// ✅ Overige repositories zonder connection string
builder.Services.AddScoped<IWedstrijdInschrijvingRepository, WedstrijdInschrijvingRepository>();
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<ITrainingAfmeldenRepository, TrainingAfmeldenRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IClubrecordRepository, ClubrecordRepository>();
builder.Services.AddScoped<IResultaatRepository, ResultaatRepository>();

// ✅ Services
builder.Services.AddScoped<CompetitieService>();
builder.Services.AddScoped<ProgrammaService>();
builder.Services.AddScoped<AfstandService>();
builder.Services.AddScoped<ZwembadService>();
builder.Services.AddScoped<GebruikerService>();
builder.Services.AddScoped<FunctieService>();

// ✅ AUTHENTICATIE MET COOKIES
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Gebruiker/LogIn";
        options.LogoutPath = "/Gebruiker/LogOut";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// ✅ Exception handling en HSTS voor productie
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// ✅ Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 👉 Authenticatie en autorisatie inschakelen
app.UseAuthentication();
app.UseAuthorization();

// ✅ Razor Pages activeren
app.MapRazorPages();

// ✅ Applicatie starten
app.Run();