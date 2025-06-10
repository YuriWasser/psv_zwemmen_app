using Core.Domain;

namespace Core.Interface
{
    public interface IResultaatRepository
    {
        public List<Resultaat> GetByGebruikerId(int gebruikerId);
        public Resultaat GetById(int resultaatId);
        public Resultaat Add(Resultaat resultaat);
        public bool Update(Resultaat resultaat);
        public bool Delete(Resultaat resultaat);
    }
}