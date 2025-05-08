using Core.Domain;

namespace Core.Interface
{
    public interface IResultaatRepository
    {
        public List<Resultaat> GetAll();
        public Resultaat GetById(int resultaatId);
        public int Add(Resultaat resultaat);
        public bool Update(Resultaat resultaat);
        public bool Delete(Resultaat resultaat);
    }
}