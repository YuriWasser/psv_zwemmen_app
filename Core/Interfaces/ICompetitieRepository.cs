using Core.Domain;

namespace Core.Interface
{
    // Interface die beschrijft welke acties je kunt uitvoeren op Competitie-objecten in de data-opslag
    public interface ICompetitieRepository
    {
        // Haal een lijst op van alle competities uit de database
        public List<Competitie> GetAll();

        // Haal één competitie op aan de hand van het unieke ID
        public Competitie GetById(int competitieId);

        // Voeg een nieuwe competitie toe aan de database en retourneer het ID van de nieuw aangemaakte competitie
        public int Add(Competitie competitie);

        // Werk een bestaande competitie bij, retourneert true als dit gelukt is
        public bool Update(Competitie competitie);

        // Verwijder een competitie uit de database, retourneert true als dit gelukt is
        public bool Delete(Competitie competitie);
    }
}