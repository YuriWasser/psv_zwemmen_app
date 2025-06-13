using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class WedstrijdInschrijvingService
{
    private readonly IWedstrijdInschrijvingRepository _wedstrijdInschrijvingRepository;
    private readonly ILogger<WedstrijdInschrijvingService> _logger;
    
    public WedstrijdInschrijvingService(IWedstrijdInschrijvingRepository wedstrijdInschrijvingRepository, ILogger<WedstrijdInschrijvingService> logger)
    {
        _wedstrijdInschrijvingRepository = wedstrijdInschrijvingRepository;
        _logger = logger;
    }
    
    public List<WedstrijdInschrijving> GetByGebruikerId(int gebruikerId)
    {
        try
        {
            if (gebruikerId <= 0)
                throw new ArgumentException("Gebruiker ID moet groter zijn dan 0", nameof(gebruikerId));
            
            return _wedstrijdInschrijvingRepository.GetByGebruikerId(gebruikerId);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van inschrijvingen voor gebruiker {GebruikerId}", gebruikerId);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van inschrijvingen", ex);
        }
    }
    
    public WedstrijdInschrijving GetById(int wedstrijdInschrijvingId)
    {
        try
        {
            return _wedstrijdInschrijvingRepository.GetById(wedstrijdInschrijvingId);
        }
        catch (WedstrijdInschrijvingNotFoundException ex)
        {
            _logger.LogError(ex, "Wedstrijd inschrijving met ID {Id} niet gevonden", wedstrijdInschrijvingId);
            throw new WedstrijdInschrijvingNotFoundException("De opgegeven inschrijving kon niet worden gevonden.", ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van inschrijving met ID {Id}", wedstrijdInschrijvingId);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van de inschrijving", ex);
        }
    }
    
    public WedstrijdInschrijving Add(WedstrijdInschrijving wedstrijdInschrijving)
    {
        try
        {
            return _wedstrijdInschrijvingRepository.Add(wedstrijdInschrijving);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen van inschrijving");
            throw new DatabaseException("Er is een fout opgetreden bij het toevoegen van de inschrijving", ex);
        }
    }
    
    public bool Update(WedstrijdInschrijving wedstrijdInschrijving)
    {
        try
        {
            return _wedstrijdInschrijvingRepository.Update(wedstrijdInschrijving);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij updaten van inschrijving");
            throw new DatabaseException("Er is een fout opgetreden bij het updaten van de inschrijving", ex);
        }
    }
    
    public bool Delete(WedstrijdInschrijving wedstrijdInschrijving)
    {
        try
        {
            return _wedstrijdInschrijvingRepository.Delete(wedstrijdInschrijving);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij verwijderen van inschrijving");
            throw new DatabaseException("Er is een fout opgetreden bij het verwijderen van de inschrijving", ex);
        }
    }
    
    public bool Exists(int gebruikerId, int programmaId, int afstandId)
    {
        try
        {
            if (gebruikerId <= 0)
                throw new ArgumentException("Gebruiker ID moet groter zijn dan 0", nameof(gebruikerId));
            if (programmaId <= 0)
                throw new ArgumentException("Programma ID moet groter zijn dan 0", nameof(programmaId));
            if (afstandId <= 0)
                throw new ArgumentException("Afstand ID moet groter zijn dan 0", nameof(afstandId));
            
            return _wedstrijdInschrijvingRepository.Exists(gebruikerId, programmaId, afstandId);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij controleren van inschrijving bestaan");
            throw new DatabaseException("Er is een fout opgetreden bij het controleren van de inschrijving", ex);
        }
    }
    
    public List<int> GetAfstandenByGebruikerEnProgramma(int gebruikerId, int programmaId)
    {
        try
        {
            if (gebruikerId <= 0)
                throw new ArgumentException("Gebruiker ID moet groter zijn dan 0", nameof(gebruikerId));
            if (programmaId <= 0)
                throw new ArgumentException("Programma ID moet groter zijn dan 0", nameof(programmaId));
            
            return _wedstrijdInschrijvingRepository.GetAfstandenByGebruikerEnProgramma(gebruikerId, programmaId);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van afstanden voor gebruiker {GebruikerId} en programma {ProgrammaId}", gebruikerId, programmaId);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van de afstanden", ex);
        }
    }
}