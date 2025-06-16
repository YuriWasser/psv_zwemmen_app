using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Feedback
{
    public int Id { get; private set; }
    
    public int GebruikerId { get; private set; }
    public int ProgrammaId { get; private set; }
    public string FeedbackText { get; private set; }

    public Feedback(int id, int gebruikerId, int programmaId, string feedbackText)
    {
        Id = id;
        GebruikerId = gebruikerId; 
        ProgrammaId = programmaId;
        FeedbackText = feedbackText;
    }
}

