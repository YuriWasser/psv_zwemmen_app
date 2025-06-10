using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class WedstrijdInschrijving
{
        public int Id { get; private set; }
        public int GebruikerId { get; private set; }
        public int ProgrammaId { get; private set; }
        public int AfstandId { get; private set; }
        public DateTime InschrijfDatum { get; private set; }

        public WedstrijdInschrijving(int id, int gebruikerId, int programmaId, int afstandId, DateTime inschrijfDatum)
        {
                Id = id;
                GebruikerId = gebruikerId;
                ProgrammaId = programmaId;
                AfstandId = afstandId;
                InschrijfDatum = inschrijfDatum;
        }
}