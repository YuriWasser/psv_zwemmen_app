using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class CompetitieService
    {
        private readonly ICompetitieRepository _competitieRepository;
        private readonly ILogger<CompetitieService> _logger;

        public CompetitieService(ICompetitieRepository competitieRepository, ILogger<CompetitieService> logger)
        {
            _competitieRepository = competitieRepository;
            _logger = logger;
        }

        public List<Competitie> GetAll()
        {
            try
            {
                return _competitieRepository.GetAll();
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij ophalen competities");
                throw new DatabaseException("Er is een fout opgetreden bij het ophalen van competities", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen competities");
                throw new Exception("Er is een fout opgetreden bij het ophalen van competities", ex);
            }
        }

        public Competitie GetById(int id)
        {
            try
            {
                var competitie = _competitieRepository.GetById(id);
                if (competitie != null)
                {
                    return competitie;
                }
                throw new Exception("Competitie niet gevonden");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen competitie met ID {Id}", id);
                throw new Exception("Er is een fout opgetreden bij het ophalen van de competitie", ex);
            }
        }

        public Competitie Add(int id, string naam, DateOnly startDatum, DateOnly eindDatum, int zwembadId, int programmaId)
        {
            try
            {
                var competitie = new Competitie(id, naam, startDatum, eindDatum, zwembadId);
                competitie.Id = _competitieRepository.Add(competitie);
                return competitie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij toevoegen van competitie");
                throw new Exception("Er is een fout opgetreden bij het toevoegen van de competitie", ex);
            }
        }

        public bool Update(Competitie competitie)
        {
            try
            {
                return _competitieRepository.Update(competitie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij updaten van competitie met ID {Id}", competitie.Id);
                throw new Exception("Er is een fout opgetreden bij het bijwerken van de competitie", ex);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var competitie = _competitieRepository.GetById(id);
                if (competitie == null)
                {
                    throw new Exception("Competitie niet gevonden voor verwijderen");
                }
                return _competitieRepository.Delete(competitie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij verwijderen van competitie met ID {Id}", id);
                throw new Exception("Er is een fout opgetreden bij het verwijderen van de competitie", ex);
            }
        }
    }
}
