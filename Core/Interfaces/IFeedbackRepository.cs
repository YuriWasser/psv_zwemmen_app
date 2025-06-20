using Core.Domain;

namespace Core.Interface
{
    public interface IFeedbackRepository
    {
        public List<Feedback> GetByZwemmerId(int gebruikerId);
        public Feedback GetById(int feedbackId);
        public Feedback Add(Feedback feedback);
        public bool Update(Feedback feedback);
        public bool Delete(Feedback feedback);

    }
}