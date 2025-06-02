using Core.Domain;

namespace Core.Interface
{
    public interface IFunctieRepository
    {
        public List<Functie> GetAll();
        public Functie GetByCode(string code);
    }
}