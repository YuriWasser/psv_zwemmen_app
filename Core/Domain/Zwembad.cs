using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Zwembad
{
    public int Id { get; set; }
    public string Naam { get; set; } 
    public string Adres { get; set; }

    public Zwembad(int id, string naam, string adres)
    {
        Id = id;
        Naam = naam;
        Adres = adres;
    }

}

