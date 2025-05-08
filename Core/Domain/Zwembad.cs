using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Zwembad
{
    public int Id { get; set; }
    public string Naam { get; set; } 
    public string Adres { get; set; }

    public Zwembad(string naam, string adres)
    {
        Naam = naam;
        Adres = adres;
    }

}

