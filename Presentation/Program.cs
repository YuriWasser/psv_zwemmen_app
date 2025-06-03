using Microsoft.Extensions.Logging;
using DataAccess.Repositories;
using Core.Interface;
using Core.Service;

var builder = WebApplication.CreateBuilder(args);

// Voeg Razor Pages toe
builder.Services.AddRazorPages();

// Haal de connection string op uit appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Repositories met dependency injection van connection string en logger
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

// Voeg overige repositories toe (zonder connection string dependency)
builder.Services.AddScoped<IWedstrijdInschrijvingRepository, WedstrijdInschrijvingRepository>();
builder.Services.AddScoped<ITrainingRepository, TrainingRepository>();
builder.Services.AddScoped<ITrainingAfmeldenRepository, TrainingAfmeldenRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IClubrecordRepository, ClubrecordRepository>();
builder.Services.AddScoped<IResultaatRepository, ResultaatRepository>();

// Voeg services toe
builder.Services.AddScoped<CompetitieService>();
builder.Services.AddScoped<ProgrammaService>();
builder.Services.AddScoped<AfstandService>();
builder.Services.AddScoped<ZwembadService>();
builder.Services.AddScoped<GebruikerService>();
// builder.Services.AddScoped<WedstrijdInschrijvingService>();
// builder.Services.AddScoped<TrainingService>();
// builder.Services.AddScoped<TrainingAfmeldingService>();
// builder.Services.AddScoped<FeedbackService>();
// builder.Services.AddScoped<ClubrecordService>();
builder.Services.AddScoped<FunctieService>();
// builder.Services.AddScoped<ResultaatService>();

var app = builder.Build();

// Stel gedrag in voor productieomgeving
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