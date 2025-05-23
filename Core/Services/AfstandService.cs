using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class AfstandService
{
    private readonly IAfstandRepository _afstandRepository;
    private readonly ILogger<AfstandService> _logger;
    
    public AfstandService(IAfstandRepository afstandRepository, ILogger<AfstandService> logger)
    {
        _afstandRepository = afstandRepository;
        _logger = logger;
    }
    
    public List<Afstand> GetAll()
    {
        try
        {
            return _afstandRepository.GetAll();
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het ophalen van afstanden");
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van de afstanden", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het ophalen van afstanden");
            throw new Exception("Er is een fout opgetreden bij het ophalen van de afstanden", ex);
        }
    }

    public Afstand GetById(int id)
    {
        try
        {
            var afstand = _afstandRepository.GetByID(id);
            if (afstand != null)
            {
                return afstand;
            }

            throw new Exception("Afstand niet gevonden");
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het ophalen van afstand met ID {Id}", id);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van de afstand", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het ophalen van afstand met ID {Id}", id);
            throw new Exception("Er is een fout opgetreden bij het ophalen van de afstand", ex);
        }
    }

    public Afstand Add(int id, int meters, string beschrijving)
    {
        try
        {
            var newAfstand = new Afstand(id, meters, beschrijving);
            var addedAfstand = _afstandRepository.Add(newAfstand);
            return addedAfstand;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het toevoegen van afstand");
            throw new DatabaseException("Er is een fout opgetreden bij het toevoegen van een afstand", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het toevoegen van afstand");
            throw new Exception("Er is een fout opgetreden bij het toevoegen van een afstand", ex);
        }
    }

    public bool Update(Afstand afstand)
    {
        try
        {
            return _afstandRepository.Update(afstand);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het updaten van afstand met ID {Id}", afstand.Id);
            throw new DatabaseException("Er is een fout opgetreden bij het updaten van de afstand", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het updaten van afstand met ID {Id}", afstand.Id);
            throw new Exception("Er is een fout opgetreden bij het updaten van de afstand", ex);
        }
    }

    public bool Delete(Afstand afstand)
    {
        try
        {
            return _afstandRepository.Delete(afstand);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het verwijderen van afstand met ID {Id}", afstand.Id);
            throw new DatabaseException("Er is een fout opgetreden bij het verwijderen van de afstand", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het verwijderen van afstand met ID {Id}", afstand.Id);
            throw new Exception("Er is een fout opgetreden bij het verwijderen van de afstand", ex);
        }
    }

    public List<Afstand> GetByProgrammaId(int programmaId)
    {
        try
        {
            return _afstandRepository.GetByProgrammaId(programmaId);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het ophalen van afstanden voor programma ID {Id}", programmaId);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van afstanden voor het programma", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het ophalen van afstanden voor programma ID {Id}", programmaId);
            throw new Exception("Er is een fout opgetreden bij het ophalen van afstanden voor het programma", ex);
        }
    }
}