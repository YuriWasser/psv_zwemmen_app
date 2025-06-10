using Core.Domain;

namespace Core.Interface
{
    public interface IWedstrijdInschrijvingRepository
    {
        public List<WedstrijdInschrijving> GetByGebruikerId(int gebruikerId);
        public WedstrijdInschrijving GetById(int wedstrijdInschrijvingId);
        public WedstrijdInschrijving Add(WedstrijdInschrijving wedstrijdInschrijving);
        public bool Update(WedstrijdInschrijving wedstrijdInschrijving);
        public bool Delete(WedstrijdInschrijving wedstrijdInschrijving);
    }
}