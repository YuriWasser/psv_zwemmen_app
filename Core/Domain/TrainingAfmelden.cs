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

        public TrainingAfmelden(int id, int gebruikerId, int trainingId)
        {
            Id = id;
            GebruikerId = gebruikerId;
            TrainingId = trainingId;
        }
        
    }
}