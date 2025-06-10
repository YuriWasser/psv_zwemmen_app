using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Resultaat
{
    public int Id { get; private set; }
    public int GebruikerId { get; private set; }
    public int ProgrammaId { get; private set; }
    public int AfstandId { get; private set; }
    public TimeSpan Tijd { get; private set; }
    public DateTime Datum { get; private set; }

    public Resultaat(int id, int gebruikerId, int programmaId, int afstandId, TimeSpan tijd, DateTime datum)
    {
        Id = id;
        GebruikerId = gebruikerId;
        ProgrammaId = programmaId;
        AfstandId = afstandId;
        Tijd = tijd;
        Datum = datum;
    }
}