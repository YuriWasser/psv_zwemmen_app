using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class Competitie
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public DateOnly StartDatum { get; set; }
        public DateOnly EindDatum { get; set; }
        public int ZwembadId { get; set; }

        public Competitie(string naam, DateOnly startDatum, DateOnly eindDatum, int zwembadId)
        {
            Naam = naam;
            StartDatum = startDatum;
            EindDatum = eindDatum;
            ZwembadId = zwembadId;
        }
    }
}