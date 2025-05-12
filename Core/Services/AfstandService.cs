using Core.Domain;
using Core.Interface;

namespace Core.Service;

public class AfstandService
{
    private readonly IAfstandRepository _afstandRepository;
    
    public AfstandService(IAfstandRepository afstandRepository)
    {
        _afstandRepository = afstandRepository;
    }
    
    public List<Afstand> GetAll()
    {
        try
        {
            return _afstandRepository.GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }
    
    public Afstand GetById(int id)
    {
        Afstand afstand = _afstandRepository.GetByID(id);
        
        if (afstand != null)
        {
            return afstand;
        }

        throw new Exception("error");
    }
    
    public Afstand Add(int id, int meters, string beschrijving)
    {
        try
        {
            Afstand afstand = new Afstand(id, meters, beschrijving);
            afstand.Id = _afstandRepository.Add(afstand);
            return afstand;
        }
        catch
        {
            throw new Exception("error");
        }
    }
    
    public bool Update(Afstand afstand)
    {
        try
        {
            return _afstandRepository.Update(afstand);
        }
        catch
        {
            throw new Exception("error");
        }
    }
    
    public bool Delete(Afstand afstand)
    {
        try
        {
            return _afstandRepository.Delete(afstand);
        }
        catch
        {
            throw new Exception("error");
        }
    }

    public List<Afstand> GetByProgrammaId(int ProgrammaId)
    {
        try
        {
            return _afstandRepository.GetByProgrammaId(ProgrammaId);
        }
        catch
        {
            throw new Exception("error");
        }
    }
  
}