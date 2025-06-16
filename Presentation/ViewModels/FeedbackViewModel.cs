namespace Presentation.ViewModels;

public class FeedbackViewModel
{
    public int Id { get; set; }
    public int GebruikerId { get; set; }
    public int ProgrammaId { get; set; }
    public string FeedbackText { get; set; }
    
    public FeedbackViewModel(int id, int gebruikerId, int programmaId, string feedbackText)
    {
        Id = id;
        GebruikerId = gebruikerId;
        ProgrammaId = programmaId;
        FeedbackText = feedbackText;
    }
}