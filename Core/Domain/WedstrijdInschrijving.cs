using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class WedstrijdInschrijving
{
        public int Id { get; set; }
        public int GebruikerId { get; set; }
        public int ProgrammaId { get; set; }
        public int AfstandId { get; set; }
        public DateTime InschrijfDatum { get; set; }

        public WedstrijdInschrijving(int id, int gebruikerId, int programmaId, int afstandId, DateTime inschrijfDatum)
        {
                Id = id;
                GebruikerId = gebruikerId;
                ProgrammaId = programmaId;
                AfstandId = afstandId;
                InschrijfDatum = inschrijfDatum;
        }
}