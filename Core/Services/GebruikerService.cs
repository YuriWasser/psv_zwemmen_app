using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class GebruikerService
{
    private readonly IGebruikerRepository _gebruikerRepository;
    private readonly ILogger<GebruikerService> _logger;
    
    public GebruikerService(IGebruikerRepository gebruikerRepository, ILogger<GebruikerService> logger)
    {
        _gebruikerRepository = gebruikerRepository;
        _logger = logger;
    }
    
    public List<Gebruiker> GetAll()
    {
        try
        {
            var result = _gebruikerRepository.GetAll();
            if (result == null)
            {
                throw new NullReferenceException("De repository retourneerde null");
            }

            return result;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruikers");
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van gebruikers", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruikers");
            throw new Exception("Er is een fout opgetreden bij het ophalen van gebruikers", ex);
        }
    }
    
    public Gebruiker GetById(int id)
    {
        try
        {
            var gebruiker = _gebruikerRepository.GetById(id);
            if (gebruiker != null)
            {
                return gebruiker;
            }

            throw new Exception("Gebruiker niet gevonden");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruiker met ID {Id}", id);
            throw new Exception("Er is een fout opgetreden bij het ophalen van de gebruiker", ex);
        }
    }

    public Gebruiker Add(int id, string gebruikersnaam, string wachtwoord, string email, string voornaam, string achternaam,
        string functieCode)
    {
        try
        {
            var newGebruiker = new Gebruiker(id, gebruikersnaam, wachtwoord, email, voornaam, achternaam, functieCode);
            var addedGebruiker = _gebruikerRepository.Add(newGebruiker);
            return addedGebruiker;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen gebruiker");
            throw new Exception("Er is een fout opgetreden bij het toevoegen van de gebruiker", ex);
        }
    }
    
    public bool Update(Gebruiker gebruiker)
    {
        try
        {
            return _gebruikerRepository.Update(gebruiker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij updaten gebruiker");
            throw new Exception("Er is een fout opgetreden bij het updaten van de gebruiker", ex);
        }
    }
    
    public bool Delete(int id)
    {
        try
        {
            var gebruiker = _gebruikerRepository.GetById(id);
            if (gebruiker == null)
            {
                throw new Exception("Gebruiker niet gevonden");
            }
            return _gebruikerRepository.Delete(gebruiker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij verwijderen gebruiker");
            throw new Exception("Er is een fout opgetreden bij het verwijderen van de gebruiker", ex);
        }
    }
     
}