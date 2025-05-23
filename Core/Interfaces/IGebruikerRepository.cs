using Core.Domain;

namespace Core.Interface
{
    public interface IGebruikerRepository
    {
        public List<Gebruiker> GetAll();
        public Gebruiker GetById(int gebruikerId);
        public Gebruiker Add(Gebruiker gebruiker);
        public bool Update(Gebruiker gebruiker);
        public bool Delete(Gebruiker gebruiker);
    }
}