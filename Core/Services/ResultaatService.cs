using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ResultaatService
{
    private readonly IResultaatRepository _resultaatRepository;
    private readonly ILogger<ResultaatService> _logger;
    
    public ResultaatService(IResultaatRepository resultaatRepository, ILogger<ResultaatService> logger)
    {
        _resultaatRepository = resultaatRepository;
        _logger = logger;
    }
    
    public List<Resultaat> GetByGebruikerId(int gebruikerId)
    {
        try
        {
            var resultaat = _resultaatRepository.GetByGebruikerId(gebruikerId);
            if (resultaat == null)
            {
                _logger.LogError("Repository returned null instead of list");
                throw new NullReferenceException("De repository retourneerde null");
            }

            return resultaat;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van resultaten voor gebruiker {GebruikerId}", gebruikerId);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van resultaten", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij ophalen van resultaten voor gebruiker {GebruikerId}", gebruikerId);
            throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van resultaten", ex);
        }
    }
    
    public Resultaat GetById(int resultaatId)
    {
        try
        {
            var resultaat = _resultaatRepository.GetById(resultaatId);
            if (resultaat == null)
            {
                _logger.LogError("Resultaat met ID {ResultaatId} niet gevonden", resultaatId);
                throw new ResultaatNotFoundException($"Resultaat met ID {resultaatId} niet gevonden");
            }

            return resultaat;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van resultaat met ID {ResultaatId}", resultaatId);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van het resultaat", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij ophalen van resultaat met ID {ResultaatId}", resultaatId);
            throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van het resultaat", ex);
        }
    }
    
    public void Add(Resultaat resultaat)
    {
        try
        {
            _resultaatRepository.Add(resultaat);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen van resultaat");
            throw new DatabaseException("Er is een fout opgetreden bij het toevoegen van het resultaat", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij toevoegen van resultaat");
            throw new Exception("Er is een onverwachte fout opgetreden bij het toevoegen van het resultaat", ex);
        }
    }
    
    public void Update(Resultaat resultaat)
    {
        try
        {
            _resultaatRepository.Update(resultaat);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij updaten van resultaat");
            throw new DatabaseException("Er is een fout opgetreden bij het updaten van het resultaat", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij updaten van resultaat");
            throw new Exception("Er is een onverwachte fout opgetreden bij het updaten van het resultaat", ex);
        }
    }
    
    public void Delete(int resultaatId)
    {
        try
        {
            var resultaat = _resultaatRepository.GetById(resultaatId);
            if (resultaat == null)
            {
                _logger.LogError("Resultaat met ID {ResultaatId} niet gevonden voor verwijdering", resultaatId);
                throw new ResultaatNotFoundException($"Resultaat met ID {resultaatId} niet gevonden");
            }
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij verwijderen van resultaat met ID {ResultaatId}", resultaatId);
            throw new DatabaseException("Er is een fout opgetreden bij het verwijderen van het resultaat", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij verwijderen van resultaat met ID {ResultaatId}", resultaatId);
            throw new Exception("Er is een onverwachte fout opgetreden bij het verwijderen van het resultaat", ex);
        }
    }
    
}