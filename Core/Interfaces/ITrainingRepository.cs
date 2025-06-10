using Core.Domain;

namespace Core.Interface
{
    public interface ITrainingRepository
    {
        public List<Training> GetByGebruikerId(int gebruikerId);
        public Training GetById(int trainingId);
        public Training Add(Training training);
        public bool Update(Training training);
        public bool Delete(Training training);
    }
}