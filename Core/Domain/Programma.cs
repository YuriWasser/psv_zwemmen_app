using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Programma
{
    public int Id { get; private set; }
    public int CompetitieId { get; private set; }
    public string Omschrijving { get; private set; }
    public DateTime Datum { get; private set; }
    public TimeSpan StartTijd { get; private set; }
    
    


    public Programma(int id, int competitieId, string omschrijving, DateTime datum, TimeSpan starttijd)
    {
        Id = id;
        CompetitieId = competitieId;
        Omschrijving = omschrijving;
        Datum = datum;
        StartTijd = starttijd;
    }
    
    
}