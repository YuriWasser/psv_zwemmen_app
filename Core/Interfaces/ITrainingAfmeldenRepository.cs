using Core.Domain;

namespace Core.Interface
{
    public interface ITrainingAfmeldenRepository
    {
        public List<TrainingAfmelden> GetAll();
        public TrainingAfmelden GetById(int trainingafmeldenId);
        public TrainingAfmelden Add(TrainingAfmelden trainingAfmelden);
        public bool Update(TrainingAfmelden trainingAfmelden);
        public bool Delete(TrainingAfmelden trainingAfmelden);
    }
}