using Core.Domain;
using Core.Interface;

namespace Core.Service
{
    public class CompetitieService
    {
        private readonly ICompetitieRepository _competitieRepository;

        public CompetitieService(ICompetitieRepository competitieRepository)
        {
            _competitieRepository = competitieRepository;
        }

        public List<Competitie> GetAll()
        {
            try
            {
                return _competitieRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public Competitie GetById(int id)
        {
            Competitie competitie = _competitieRepository.GetById(id);

            if (competitie != null)
            {
                return competitie;
            }

            throw new Exception("error");
        }

        public Competitie Add(int id, string naam, DateOnly startDatum, DateOnly eindDatum, int zwembadId,
            int programmaId)
        {
            try
            {
                Competitie competitie = new Competitie(id, naam, startDatum, eindDatum, zwembadId, programmaId);
                competitie.Id = _competitieRepository.Add(competitie);
                return competitie;
            }
            catch
            {
                throw new Exception("error");
            }
        }

        public bool Update(Competitie competitie)
        {
            try
            {
                return _competitieRepository.Update(competitie);
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int Id)
        {
            try
            {
                var competitie = _competitieRepository.GetById(Id);
                return _competitieRepository.Delete(competitie);
            }
            catch
            {
                return false;
            }
        }
    }
}