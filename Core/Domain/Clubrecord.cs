using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class Clubrecord
    {
        public int Id { get; set; }
        public int GebruikerId { get; set; }
        public int AfstandId { get; set; }
        public TimeSpan Tijd { get; set; }
        public DateTime Datum { get; set; }


        public Clubrecord(int id, int gebruikerId, int afstandId, TimeSpan tijd, DateTime datum)
        {
            Id = id;
            GebruikerId = gebruikerId;
            AfstandId = afstandId;
            Tijd = tijd;
            Datum = datum;
        }
    }
}