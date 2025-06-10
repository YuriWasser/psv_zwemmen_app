using Core.Domain;

namespace Core.Interface
{
    public interface IWedstrijdInschrijvingRepository
    {
        public List<WedstrijdInschrijving> GetAll();
        public WedstrijdInschrijving GetById(int wedstrijdInschrijvingId);
        public WedstrijdInschrijving Add(WedstrijdInschrijving wedstrijdInschrijving);
        public bool Update(WedstrijdInschrijving wedstrijdInschrijving);
        public bool Delete(WedstrijdInschrijving wedstrijdInschrijving);
    }
}