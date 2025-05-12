using Core.Domain;
using Core.Interface;

namespace Core.Service;

public class ZwembadService
{
    private readonly IZwembadRepository _zwembadRepository;
    
    public ZwembadService(IZwembadRepository zwembadRepository)
    {
        _zwembadRepository = zwembadRepository;
    }
    
    public List<Zwembad> GetAll()
    {
        try
        {
            return _zwembadRepository.GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }
    
    public Zwembad GetById(int id)
    {
        Zwembad zwembad = _zwembadRepository.GetById(id);
        
        if (zwembad != null)
        {
            return zwembad;
        }

        throw new Exception("error");
    }
    
    public Zwembad Add(int id, string naam, string adres)
    {
        try
        {
            Zwembad zwembad = new Zwembad(id, naam, adres);
            zwembad.Id = _zwembadRepository.Add(zwembad);
            return zwembad;
        }
        catch
        {
            throw new Exception("error");
        }
    }
    
    public bool Update(Zwembad zwembad)
    {
        try
        {
            return _zwembadRepository.Update(zwembad);
        }
        catch
        {
            throw new Exception("error");
        }
    }
    
    public bool Delete(Zwembad zwembad)
    {
        try
        {
            return _zwembadRepository.Delete(zwembad);
        }
        catch
        {
            throw new Exception("error");
        }
    }
}