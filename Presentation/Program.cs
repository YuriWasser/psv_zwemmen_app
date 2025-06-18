using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Repositories;
using Core.Interface;
using Core.Service;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity; // 👈 Bovenaan je bestand zetten als dat nog niet is toegevoegd
using Core.Domain; // 👈 Voor toegang tot Gebruiker


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

builder.Services.AddScoped<IWedstrijdInschrijvingRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<WedstrijdInschrijvingRepository>>();
    return new WedstrijdInschrijvingRepository(connectionString, logger);
});

builder.Services.AddScoped<ITrainingRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<TrainingRepository>>();
    return new TrainingRepository(connectionString, logger);
});

builder.Services.AddScoped<IResultaatRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<ResultaatRepository>>();
    return new ResultaatRepository(connectionString, logger);
});

builder.Services.AddScoped<IClubrecordRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<ClubrecordRepository>>();
    return new ClubrecordRepository(connectionString, logger);
});

builder.Services.AddScoped<IFeedbackRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<FeedbackRepository>>();
    return new FeedbackRepository(connectionString, logger);
});

builder.Services.AddScoped<IAfstandPerProgrammaRepository>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<AfstandPerProgrammaRepository>>();
    return new AfstandPerProgrammaRepository(connectionString, logger);
});

// ✅ Services
builder.Services.AddScoped<CompetitieService>();
builder.Services.AddScoped<ProgrammaService>();
builder.Services.AddScoped<AfstandService>();
builder.Services.AddScoped<ZwembadService>();
builder.Services.AddScoped<GebruikerService>();
builder.Services.AddScoped<FunctieService>();
builder.Services.AddScoped<WedstrijdInschrijvingService>();
builder.Services.AddScoped<TrainingService>();
builder.Services.AddScoped<ResultaatService>();
builder.Services.AddScoped<ClubrecordService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<AfstandPerProgrammaService>();

// ✅ AUTHENTICATIE MET COOKIES
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Gebruiker/LogIn";
        options.LogoutPath = "/Gebruiker/LogOut";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = context =>
            {
                var identity = (ClaimsIdentity)context.Principal.Identity;

                // Zorg dat de 'role' claim wordt gezien als rol
                var roleClaims = identity.FindAll("role").ToList();
                foreach (var claim in roleClaims)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, claim.Value));
                }

                return Task.CompletedTask;
            }
        };
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