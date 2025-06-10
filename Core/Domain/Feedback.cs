using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Feedback
{
    public int Id { get; private set; }
    public int ZwemmerId { get; private set; }
    public int TrainerId { get; private set; }
    public int ProgrammaId { get; private set; }
    public string FeedbackText { get; private set; }

    public Feedback(int id, int zwemmerId, int trainerId, int programmaId, string feedbackText)
    {
        Id = id;
        ZwemmerId = zwemmerId;
        TrainerId = trainerId;
        ProgrammaId = programmaId;
        FeedbackText = feedbackText;
    }
}

