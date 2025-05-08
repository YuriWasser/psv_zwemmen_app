using Core.Domain;

namespace Core.Interface
{
    public interface IClubrecordRepository
    {
        public List<Clubrecord> GetAll();
        public Clubrecord GetByID(int clubrecordId);
        public int Add(Clubrecord clubrecord);
        public bool Update(Clubrecord clubrecord);
        public bool Delete(Clubrecord clubrecord);
    }
}