using Core.Domain;

namespace Core.Interface
{
    public interface ITrainingRepository
    {
        public List<Training> GetActieveTrainingen(int gebruikerId);
        public Training GetById(int trainingId);
        public Training Add(Training training);
        public bool Update(Training training);
        public bool Delete(Training training);
    }
}