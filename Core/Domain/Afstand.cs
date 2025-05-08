using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
    public class Afstand
    {
        public int Id { get; set; }
        public int Meters { get; set; }
        public string? Beschrijving { get; set; }

        public Afstand(int meters, string beschrijving)
        {
            Meters = meters;
            Beschrijving = beschrijving;
        }
    }
}