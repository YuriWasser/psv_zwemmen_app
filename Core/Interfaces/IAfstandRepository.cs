using Core.Domain;

namespace Core.Interface
{
    public interface IAfstandRepository
    {
        public List<Afstand> GetAll();
        public Afstand GetByID(int afstandId);
        public Afstand Add(Afstand afstand);
        public bool Update(Afstand afstand);
        public bool Delete(Afstand afstand);
        public List<Afstand> GetByProgrammaId(int programmaId);
    }
}