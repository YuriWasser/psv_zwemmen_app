using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class Clubrecord
    {
        public int Id { get; private set; }
        public int GebruikerId { get; private set; }
        public int AfstandId { get; private set; }
        public TimeSpan Tijd { get; private set; }
        public DateTime Datum { get; private set; }


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