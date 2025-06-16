using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class TrainingService
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly ILogger<TrainingService> _logger;
    
    public TrainingService(ITrainingRepository trainingRepository, ILogger<TrainingService> logger)
    {
        _trainingRepository = trainingRepository;
        _logger = logger;
    }
    
    public List<Training> GetActieveTrainingen(int gebruikerId)
    {
        try
        {
            var result = _trainingRepository.GetActieveTrainingen(gebruikerId);
            if (result == null)
            {
                _logger.LogError("Repository returned null instead of list");
                throw new NullReferenceException("De repository retourneerde null");
            }

            return result;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van actieve trainingen");
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van actieve trainingen", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij ophalen van actieve trainingen");
            throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van actieve trainingen", ex);
        }
    }
    
    public Training GetById(int id)
    {
        try
        {
            var training = _trainingRepository.GetById(id);
            if (training != null)
            {
                return training;
            }

            throw new Exception("Training niet gevonden");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van training met ID {Id}", id);
            throw new Exception($"Er is een fout opgetreden bij het ophalen van de training met ID {id}", ex);
        }
    }
    
    public Training Add(Training training)
    {
        try
        {
            if (training == null)
            {
                throw new ArgumentNullException(nameof(training), "Training mag niet null zijn");
            }

            return _trainingRepository.Add(training);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen van training");
            throw new DatabaseException("Er is een fout opgetreden bij het toevoegen van de training", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij toevoegen van training");
            throw new Exception("Er is een onverwachte fout opgetreden bij het toevoegen van de training", ex);
        }
    }
    
    public void Update(Training training)
    {
        try
        {
            if (training == null)
            {
                throw new ArgumentNullException(nameof(training), "Training mag niet null zijn");
            }

            _trainingRepository.Update(training);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij updaten van training");
            throw new DatabaseException("Er is een fout opgetreden bij het updaten van de training", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij updaten van training");
            throw new Exception("Er is een onverwachte fout opgetreden bij het updaten van de training", ex);
        }
    }
    
    public void Delete(int id)
    {
        try
        {
            var training = _trainingRepository.GetById(id);
            if (training == null)
            {
                throw new Exception("Training niet gevonden voor verwijderen");
            }

            _trainingRepository.Delete(training);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij verwijderen van training met ID {Id}", id);
            throw new DatabaseException("Er is een fout opgetreden bij het verwijderen van de training", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij verwijderen van training met ID {Id}", id);
            throw new Exception("Er is een onverwachte fout opgetreden bij het verwijderen van de training", ex);
        }
    }
}