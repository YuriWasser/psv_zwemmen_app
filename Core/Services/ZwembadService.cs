using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ZwembadService
{
    private readonly IZwembadRepository _zwembadRepository;
    private readonly ILogger<ZwembadService> _logger;

    public ZwembadService(IZwembadRepository zwembadRepository, ILogger<ZwembadService> logger)
    {
        _zwembadRepository = zwembadRepository;
        _logger = logger;
    }

    public List<Zwembad> GetAll()
    {
        try
        {
            return _zwembadRepository.GetAll();
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het ophalen van zwembaden");
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van de zwembaden", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het ophalen van zwembaden");
            throw new Exception("Er is een fout opgetreden bij het ophalen van de zwembaden", ex);
        }
    }

    public Zwembad GetById(int id)
    {
        try
        {
            var zwembad = _zwembadRepository.GetById(id);
            if (zwembad != null)
            {
                return zwembad;
            }

            throw new Exception("Zwembad niet gevonden");
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het ophalen van zwembad met ID {Id}", id);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van het zwembad", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het ophalen van zwembad met ID {Id}", id);
            throw new Exception("Er is een fout opgetreden bij het ophalen van het zwembad", ex);
        }
    }

    public Zwembad Add(int id, string naam, string adres)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(naam))
                throw new ArgumentException("Naam mag niet leeg zijn", nameof(naam));
            if (string.IsNullOrWhiteSpace(adres))
                throw new ArgumentException("Adres mag niet leeg zijn", nameof(adres));
            
            var newZwembad = new Zwembad(id, naam, adres);
            var addedZwembad = _zwembadRepository.Add(newZwembad);
            return addedZwembad;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het toevoegen van zwembad");
            throw new DatabaseException("Er is een fout opgetreden bij het toevoegen van een zwembad", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het toevoegen van zwembad");
            throw new Exception("Er is een fout opgetreden bij het toevoegen van een zwembad", ex);
        }
    }

    public bool Update(Zwembad zwembad)
    {
        try
        {
            return _zwembadRepository.Update(zwembad);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het updaten van zwembad met ID {Id}", zwembad.Id);
            throw new DatabaseException("Er is een fout opgetreden bij het updaten van het zwembad", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het updaten van zwembad met ID {Id}", zwembad.Id);
            throw new Exception("Er is een fout opgetreden bij het updaten van het zwembad", ex);
        }
    }

    public bool Delete(Zwembad zwembad)
    {
        try
        {
            return _zwembadRepository.Delete(zwembad);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij het verwijderen van zwembad met ID {Id}", zwembad.Id);
            throw new DatabaseException("Er is een fout opgetreden bij het verwijderen van het zwembad", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij het verwijderen van zwembad met ID {Id}", zwembad.Id);
            throw new Exception("Er is een fout opgetreden bij het verwijderen van het zwembad", ex);
        }
    }
}