using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Resultaat
{
    public int Id { get; set; }
    public int GebruikerId { get; set; }
    public int ProgrammaId { get; set; }
    public int AfstandId { get; set; }
    public TimeSpan Tijd { get; set; }
    public DateTime Datum { get; set; }

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