using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class TrainingAfmelden
    {
        public int Id { get; private set; }
        public int GebruikerId { get; private set; }
        public int TrainingId { get; private set; }

        public TrainingAfmelden(int id, int gebruikerId, int trainingId)
        {
            Id = id;
            GebruikerId = gebruikerId;
            TrainingId = trainingId;
        }
        
    }
}