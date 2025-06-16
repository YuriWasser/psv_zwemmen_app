using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class FeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ILogger<FeedbackService> _logger;
    
    public FeedbackService(IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger)
    {
        _feedbackRepository = feedbackRepository;
        _logger = logger;
    }
    
    public List<Feedback> GetByZwemmerId(int gebruikerId)
    {
        try
        {
            var feedbacks = _feedbackRepository.GetByZwemmerId(gebruikerId);
            if (feedbacks == null || feedbacks.Count == 0)
            {
                _logger.LogWarning($"Geen feedback gevonden voor zwemmer met ID {gebruikerId}");
                return new List<Feedback>();
            }
            return feedbacks;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, $"Fout bij ophalen van feedback voor zwemmer met ID {gebruikerId}");
            throw new DatabaseException($"Kon feedback niet ophalen voor zwemmer met ID {gebruikerId}", ex);
        }
    }
    
    public Feedback GetById(int feedbackId)
    {
        try
        {
            var feedback = _feedbackRepository.GetById(feedbackId);
            if (feedback == null)
            {
                _logger.LogWarning($"Feedback met ID {feedbackId} niet gevonden");
                throw new Exception($"Feedback met ID {feedbackId} niet gevonden");
            }
            return feedback;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, $"Fout bij ophalen van feedback met ID {feedbackId}");
            throw new DatabaseException($"Kon feedback niet ophalen met ID {feedbackId}", ex);
        }
    }
    
    public Feedback Add(Feedback feedback)
    {
        try
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback), "Feedback mag niet null zijn");
            }
            return _feedbackRepository.Add(feedback);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen van feedback");
            throw new DatabaseException("Kon feedback niet toevoegen", ex);
        }
    }
    
    public bool Update(Feedback feedback)
    {
        try
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback), "Feedback mag niet null zijn");
            }
            return _feedbackRepository.Update(feedback);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij updaten van feedback");
            throw new DatabaseException("Kon feedback niet updaten", ex);
        }
    }
    
    public bool Delete(Feedback feedback)
    {
        try
        {
            if (feedback == null)
            {
                throw new ArgumentNullException(nameof(feedback), "Feedback mag niet null zijn");
            }
            return _feedbackRepository.Delete(feedback);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij verwijderen van feedback");
            throw new DatabaseException("Kon feedback niet verwijderen", ex);
        }
    }
}