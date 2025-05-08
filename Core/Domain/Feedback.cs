using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Feedback
{
    public int Id { get; set; }
    public int ZwemmerId { get; set; }
    public int TrainerId { get; set; }
    public int ProgrammaId { get; set; }
    public string? FeedbackText { get; set; }

    public Feedback(int zwemmerId, int trainerId, int programmaId, string feedbackText)
    {
        ZwemmerId = zwemmerId;
        TrainerId = trainerId;
        ProgrammaId = programmaId;
        FeedbackText = feedbackText;
    }
}

