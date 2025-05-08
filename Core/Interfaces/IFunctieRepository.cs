using Core.Domain;

namespace Core.Interface
{
    public interface IFunctieRepository
    {
        public List<Functie> GetAll();
        public Functie GetById(int functieId);
        public int Add(Functie functie);
        public bool Update(Functie functie);
        public bool Delete(Functie functie);

    }
}