using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    // Klasse die een competitie representeert
    public class Competitie
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public DateOnly StartDatum { get; set; }
        public DateOnly EindDatum { get; set; }
        public int ZwembadId { get; set; }

        // Constructor om een nieuwe Competitie aan te maken met alle vereiste gegevens
        public Competitie(int id, string naam, DateOnly startDatum, DateOnly eindDatum, int zwembadId)
        {
            Id = id; // Stel het ID van de competitie in
            Naam = naam;
            StartDatum = startDatum;
            EindDatum = eindDatum;
            ZwembadId = zwembadId;
        }
    }
}