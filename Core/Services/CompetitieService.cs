using Core.Domain;
using Core.Interface;

namespace Core.Service
{
    // Serviceklasse die de logica bevat voor het werken met Competitie-objecten
    public class CompetitieService
    {
        // Repository die verantwoordelijk is voor database-acties rondom competitie
        private readonly ICompetitieRepository _competitieRepository;

        // Constructor met dependency injection van het repository
        public CompetitieService(ICompetitieRepository competitieRepository)
        {
            _competitieRepository = competitieRepository;
        }

        // Haal alle competities op via het repository
        public List<Competitie> GetAll()
        {
            try
            {
                return _competitieRepository.GetAll();
            }
            catch (Exception ex)
            {
                //gooit een foutmelding als er iets misgaat
                throw new Exception($"{ex.Message}");
            }
        }

        // Haal een specifieke competitie op via ID
        public Competitie GetById(int id)
        {
            Competitie competitie = _competitieRepository.GetById(id);

            if (competitie != null)
            {
                return competitie;
            }

            
            throw new Exception("error");
        }

        // Maak een nieuwe competitie aan en voeg deze toe via het repository
        public Competitie Add(int id, string naam, DateOnly startDatum, DateOnly eindDatum, int zwembadId, int programmaId)
        {
            try
            {
                // Maak een nieuw Competitie-object aan met de meegegeven gegevens
                Competitie competitie = new Competitie(id, naam, startDatum, eindDatum, zwembadId);
                
                // Voeg toe via repository en krijg gegenereerd ID terug
                competitie.Id = _competitieRepository.Add(competitie);

                return competitie;
            }
            catch
            {
                throw new Exception("error");
            }
        }

        // Werk een bestaande competitie bij
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

        // Verwijder een competitie op basis van ID
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