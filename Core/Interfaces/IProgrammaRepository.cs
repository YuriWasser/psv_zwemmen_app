using Core.Domain;

namespace Core.Interface
{
    public interface IProgrammaRepository
    {
        public Programma GetById(int programmaId);
        public Programma Add(Programma programma);
        public bool Update(Programma programma);
        public bool Delete(Programma programma);
        public List<Programma> GetByCompetitieId(int competitieId);

    }
}