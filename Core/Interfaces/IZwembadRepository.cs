using Core.Domain;

namespace Core.Interface
{
    public interface IZwembadRepository
    {
        public List<Zwembad> GetAll();
        public Zwembad GetById(int zwembadId);
        public int Add(Zwembad zwembad);
        public bool Update(Zwembad zwembad);
        public bool Delete(Zwembad zwembad);
    }
}