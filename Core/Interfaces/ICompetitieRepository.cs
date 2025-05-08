using Core.Domain;

namespace Core.Interface
{
    public interface ICompetitieRepository
    {
        public List<Competitie> GetAll();
        
        public Competitie GetById(int competitieId);
        
        public int Add(Competitie competitie);
        
        public bool Update(Competitie competitie);
        
        public bool Delete(Competitie competitie);
    }
}