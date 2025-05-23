using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Zwembad
{
    public int Id { get; private set; }
    public string Naam { get; private set; } 
    public string Adres { get; private set; }

    public Zwembad(int id, string naam, string adres)
    {
        Id = id;
        Naam = naam;
        Adres = adres;
    }

}

