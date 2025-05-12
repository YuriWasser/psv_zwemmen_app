using Core.Domain;

namespace Core.Interface
{
    public interface IProgrammaRepository
    {
        public List<Programma> GetAll();
        public Programma GetById(int programmaId);
        public int Add(Programma programma);
        public bool Update(Programma programma);
        public bool Delete(Programma programma);
    }
}