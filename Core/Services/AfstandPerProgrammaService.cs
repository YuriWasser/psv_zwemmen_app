using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class AfstandPerProgrammaService
{
    private readonly IAfstandPerProgrammaRepository _afstandPerProgrammaRepository;
    private readonly ILogger<AfstandPerProgrammaService> _logger;
    
    public AfstandPerProgrammaService(IAfstandPerProgrammaRepository afstandPerProgrammaRepository, ILogger<AfstandPerProgrammaService> logger)
    {
        _afstandPerProgrammaRepository = afstandPerProgrammaRepository;
        _logger = logger;
    }
    
    public void AddAfstandPerProgramma(int programmaId, int afstandId, int volgorde)
    {
        try
        {
            _afstandPerProgrammaRepository.Add(programmaId, afstandId, volgorde);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen van afstand per programma (programmaId={ProgrammaId}, afstandId={AfstandId}, volgorde={Volgorde})",
                programmaId, afstandId, volgorde);
            throw new DatabaseException("Er is een fout opgetreden bij het toevoegen van afstand per programma", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij toevoegen van afstand per programma");
            throw new Exception("Er is een onverwachte fout opgetreden bij het toevoegen van afstand per programma", ex);
        }
    }
    
    public List<Afstand> GetByProgrammaId(int programmaId)
    {
        try
        {
            return _afstandPerProgrammaRepository.GetByProgrammaId(programmaId);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen van afstanden voor programma ID {ProgrammaId}", programmaId);
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van afstanden per programma", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Onverwachte fout bij ophalen van afstanden voor programma ID {ProgrammaId}", programmaId);
            throw new Exception("Er is een onverwachte fout opgetreden bij het ophalen van afstanden per programma", ex);
        }
    }
}