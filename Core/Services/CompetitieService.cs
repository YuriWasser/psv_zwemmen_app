using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class CompetitieService
    {
        private readonly IProgrammaRepository _programmaRepository;
        private readonly ICompetitieRepository _competitieRepository;
        private readonly ILogger<CompetitieService> _logger;

        public CompetitieService(ICompetitieRepository competitieRepository, ILogger<CompetitieService> logger, 
            IProgrammaRepository programmaRepository)
        
        {
            _competitieRepository = competitieRepository;
            _programmaRepository = programmaRepository;
            _logger = logger;
        }

        public List<Competitie> GetActieveCompetities()
        {
            try
            {
                var result = _competitieRepository.GetActieveCompetities();
                if (result == null)
                {
                    _logger.LogError("Repository returned null instead of list");
                    throw new NullReferenceException("De repository retourneerde null");
                }

                return result;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van actieve competities");
                throw new DatabaseException("Er is een fout opgetreden bij het ophalen van actieve competities", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij ophalen van actieve competities");
                throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van actieve competities", ex);
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

        public Competitie Add(int id, string naam, DateOnly startDatum, DateOnly eindDatum, int zwembadId,
            int programmaId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(naam))
                    throw new ArgumentException("Naam mag niet leeg zijn", nameof(naam));
                if (eindDatum < startDatum)
                    throw new ArgumentException("Einddatum mag niet voor de startdatum liggen", nameof(eindDatum));
                if (zwembadId <= 0)
                    throw new ArgumentException("Zwembad ID moet groter zijn dan 0", nameof(zwembadId));
                if (programmaId < 0)
                    throw new ArgumentException("Programma ID moet groter zijn dan 0", nameof(programmaId));
                
                var newComp = new Competitie(id, naam, startDatum, eindDatum, zwembadId, programmaId);
                var addedComp = _competitieRepository.Add(newComp);
                return addedComp;
            }
            catch (ArgumentException ex)
            {
                throw; 
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

        public List<Programma> GetProgrammaVoorCompetitie(int competitieId)
        {
            try
            {
                var result = _competitieRepository.GetProgrammaVoorCompetitie(competitieId);
                if (result == null)
                {
                    _logger.LogError("Geen programma's gevonden voor competitie ID {CompetitieId}", competitieId);
                    throw new Exception("Geen programma's gevonden voor deze competitie");
                }

                return result;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Databasefout bij ophalen van programma's voor competitie {CompetitieId}",
                    competitieId);
                throw new DatabaseException(
                    "Er is een databasefout opgetreden bij het ophalen van programma's voor de competitie", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen programma's voor competitie {CompetitieId}", competitieId);
                throw new Exception("Er is een fout opgetreden bij het ophalen van programma's voor de competitie", ex);
            }
        }

        public Programma GetProgrammaById(int id)
        {
            try
            {
                var programma = _programmaRepository.GetById(id);
                if (programma != null)
                {
                    return programma;
                }

                _logger.LogWarning("Programma met ID {Id} niet gevonden", id);
                throw new Exception("Programma niet gevonden");
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Databasefout bij ophalen van programma met ID {Id}", id);
                throw new DatabaseException("Er is een databasefout opgetreden bij het ophalen van het programma", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen programma met ID {Id}", id);
                throw new Exception("Er is een fout opgetreden bij het ophalen van het programma", ex);
            }
        }
        
        public Competitie AddMetProgramma(Competitie competitie)
        {
            try
            {
                if (competitie == null)
                    throw new ArgumentNullException(nameof(competitie), "Competitie mag niet null zijn");

                var addedComp = _competitieRepository.AddMetProgramma(competitie);
                return addedComp;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Competitie is null bij toevoegen");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij toevoegen van competitie met programma");
                throw new Exception("Er is een fout opgetreden bij het toevoegen van de competitie met programma", ex);
            }
        }
    }
}