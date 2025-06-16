using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ClubrecordService
{
    private readonly IClubrecordRepository _clubrecordRepository;
    private readonly ILogger<ClubrecordService> _logger;
    
    public ClubrecordService(IClubrecordRepository clubrecordRepository, ILogger<ClubrecordService> logger)
    {
        _clubrecordRepository = clubrecordRepository;
        _logger = logger;
    }
    
    public List<Clubrecord> GetAll()
    {
        try
        {
            var result = _clubrecordRepository.GetAll();
            if (result == null)
            {
                _logger.LogError("Repository returned null instead of list");
                throw new NullReferenceException("De repository retourneerde null");
            }

            return result;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van clubrecords");
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van clubrecords", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij ophalen van clubrecords");
            throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van clubrecords", ex);
        }
    }
    
    public Clubrecord GetById(int id)
    {
        try
        {
            var clubrecord = _clubrecordRepository.GetByID(id);
            if (clubrecord != null)
            {
                return clubrecord;
            }

            throw new Exception("Clubrecord niet gevonden");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van clubrecord met ID {Id}", id);
            throw new Exception($"Er is een fout opgetreden bij het ophalen van clubrecord met ID {id}", ex);
        }
    }
    
    public Clubrecord Add(Clubrecord clubrecord)
    {
        try
        {
            if (clubrecord == null)
            {
                throw new ArgumentNullException(nameof(clubrecord), "Clubrecord mag niet null zijn");
            }

            return _clubrecordRepository.Add(clubrecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen van clubrecord");
            throw new Exception("Er is een fout opgetreden bij het toevoegen van het clubrecord", ex);
        }
    }
    
    public bool Update(Clubrecord clubrecord)
    {
        try
        {
            if (clubrecord == null)
            {
                throw new ArgumentNullException(nameof(clubrecord), "Clubrecord mag niet null zijn");
            }

            return _clubrecordRepository.Update(clubrecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij updaten van clubrecord");
            throw new Exception("Er is een fout opgetreden bij het updaten van het clubrecord", ex);
        }
    }
    
    public bool Delete(Clubrecord clubrecord)
    {
        try
        {
            if (clubrecord == null)
            {
                throw new ArgumentNullException(nameof(clubrecord), "Clubrecord mag niet null zijn");
            }

            return _clubrecordRepository.Delete(clubrecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij verwijderen van clubrecord");
            throw new Exception("Er is een fout opgetreden bij het verwijderen van het clubrecord", ex);
        }
    }
}