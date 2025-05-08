using Microsoft.EntityFrameworkCore;
using DataAccess;
using Core.Interface;
using DataAccess.Repositories;
using Core.Service;

var builder = WebApplication.CreateBuilder(args);

// Voeg Razor Pages toe aan de service container (voor weergave in de browser)
builder.Services.AddRazorPages();



// Voeg alle repositories toe aan de DI-container
// Deze zorgen voor de communicatie met de database (DataAccess laag)
builder.Services.AddScoped<ICompetitieRepository, CompetitieRepository>();
builder.Services.AddScoped<IProgrammaRepository, ProgrammaRepository>();
builder.Services.AddScoped<IAfstandRepository, AfstandRepository>();
builder.Services.AddScoped<IGebruikerRepository, GebruikerRepository>();
builder.Services.AddScoped<IWedstrijdInschrijvingRepository, WedstrijdInschrijvingRepository>();
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<ITrainingAfmeldenRepository, TrainingAfmeldenRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IClubrecordRepository, ClubrecordRepository>();
builder.Services.AddScoped<IFunctieRepository, FunctieRepository>();
builder.Services.AddScoped<IResultaatRepository, ResultaatRepository>();
builder.Services.AddScoped<IZwembadRepository, ZwembadRepository>();

// Voeg services toe aan de DI-container (Core laag)
// Deze voeren de logica uit en maken gebruik van repositories
builder.Services.AddScoped<CompetitieService>();
builder.Services.AddScoped<ProgrammaService>();
//builder.Services.AddScoped<AfstandService>();
//builder.Services.AddScoped<GebruikerService>();
//builder.Services.AddScoped<WedstrijdInschrijvingService>();
//builder.Services.AddScoped<TrainingService>();
//builder.Services.AddScoped<TrainingAfmeldingService>();
//builder.Services.AddScoped<FeedbackService>();
//builder.Services.AddScoped<ClubrecordService>();
//builder.Services.AddScoped<FunctieService>();
//builder.Services.AddScoped<ResultaatService>();
//builder.Services.AddScoped<ZwembadService>();

var app = builder.Build();

// Stel gedrag in voor productieomgeving
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Toon gebruikersvriendelijke foutpagina
    app.UseHsts(); // Voeg security header toe voor HTTPS
}

app.UseHttpsRedirection(); // Forceer HTTPS
app.UseStaticFiles(); // Laad CSS, JS, afbeeldingen etc.
app.UseRouting();     // Schakel routing in (nodig voor Razor Pages)
app.UseAuthorization(); // Inschakelen van beveiliging (optioneel)

app.MapRazorPages(); // Koppel Razor Pages aan de routes

app.Run(); // Start de applicatie
