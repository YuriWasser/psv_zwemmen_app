using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class TrainingAfmelden
    {
        public int Id { get; set; }
        public int GebruikerId { get; set; }
        public int TrainingId { get; set; }

        public TrainingAfmelden(int gebruikerId, int trainingId)
        {
            GebruikerId = gebruikerId;
            TrainingId = trainingId;
        }
        
    }
}