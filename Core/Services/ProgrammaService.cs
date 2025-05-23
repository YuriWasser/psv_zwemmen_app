using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class ProgrammaService
    {
        private readonly IProgrammaRepository _programmaRepository;
        private readonly ILogger<ProgrammaService> _logger;

        public ProgrammaService(IProgrammaRepository programmaRepository, ILogger<ProgrammaService> logger)
        {
            _programmaRepository = programmaRepository;
            _logger = logger;
        }

        public List<Programma> GetAll()
        {
            try
            {
                return _programmaRepository.GetAll();
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van alle programma's");
                throw new DatabaseException("Er is een fout opgetreden bij het ophalen van programma's", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij ophalen van programma's");
                throw new Exception("Er is een fout opgetreden bij het ophalen van programma's", ex);
            }
        }

        public Programma GetById(int id)
        {
            try
            {
                return _programmaRepository.GetById(id);
            }
            catch (ProgrammaNotFoundException ex)
            {
                _logger.LogWarning(ex, "Programma met ID {Id} niet gevonden", id);
                throw new Exception("Het gevraagde programma kon niet worden gevonden.", ex);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van programma met ID {Id}", id);
                throw new DatabaseException("Er is een fout opgetreden bij het ophalen van het programma", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij ophalen van programma met ID {Id}", id);
                throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van het programma", ex);
            }
        }

        public Programma Add(int id, int competitieId, string omschrijving, DateTime datum, TimeSpan starttijd)
        {
            try
            {
                var newProg = new Programma(id, competitieId, omschrijving, datum, starttijd);
                var addedProg = _programmaRepository.Add(newProg);
                return addedProg;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij toevoegen van programma");
                throw new DatabaseException("Er is een fout opgetreden bij het toevoegen van een programma", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij toevoegen van programma");
                throw new Exception("Er is een fout opgetreden bij het toevoegen van een programma", ex);
            }
        }

        public bool Update(Programma programma)
        {
            try
            {
                return _programmaRepository.Update(programma);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij updaten van programma met ID {Id}", programma.Id);
                throw new DatabaseException("Er is een fout opgetreden bij het updaten van het programma", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij updaten van programma met ID {Id}", programma.Id);
                throw new Exception("Er is een fout opgetreden bij het updaten van het programma", ex);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var programma = _programmaRepository.GetById(id);
                return _programmaRepository.Delete(programma);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Fout bij verwijderen van programma met ID {Id}", id);
                throw new DatabaseException("Er is een fout opgetreden bij het verwijderen van het programma", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij verwijderen van programma met ID {Id}", id);
                throw new Exception("Er is een fout opgetreden bij het verwijderen van het programma", ex);
            }
        }
    }
}