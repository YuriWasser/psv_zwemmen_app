using Core.Domain;

namespace Core.Interface
{
    
    public interface ICompetitieRepository
    {
        public List<Competitie> GetAll();
        public Competitie GetById(int competitieId);
        public Competitie Add(Competitie competitie);
        public bool Update(Competitie competitie);
        public bool Delete(Competitie competitie);
        
        List<Programma> GetProgrammaVoorCompetitie(int competitieId);
        Programma GetProgrammaById(int id);
    }
}