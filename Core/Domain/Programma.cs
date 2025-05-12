using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Programma
{
    public int Id { get; set; }
    public int CompetitieId { get; set; }
    public string Omschrijving { get; set; }
    public DateTime Datum { get; set; }
    public TimeSpan StartTijd { get; set; }

    public Programma(int competitieId, string omschrijving, DateTime datum, TimeSpan starttijd)
    {
        CompetitieId = competitieId;
        Omschrijving = omschrijving;
        Datum = datum;
        StartTijd = starttijd;
    }
}