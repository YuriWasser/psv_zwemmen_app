using Core.Domain;

namespace Core.Interface;

public interface IAfstandPerProgrammaRepository
{
    void Add(int programmaId, int afstandId, int volgorde);
    public List<Afstand> GetByProgrammaId(int programmaId);
}