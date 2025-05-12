using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;
public class Gebruiker
{
    public int Id { get; set; }
    public string Gebruikersnaam { get; set; } 
    public string Wachtwoord { get; set; }
    public string Email { get; set; } 
    public string Voornaam { get; set; }
    public string Achternaam { get; set; }
    public string FunctieCode { get; set; }


    public Gebruiker(int id, string gebruikersnaam, string wachtwoord, string email, string voornaam, string achternaam,
        string functieCode)
    {
        Id = id;
        Gebruikersnaam = gebruikersnaam;
        Wachtwoord = wachtwoord;
        Email = email;
        Voornaam = voornaam;
        Achternaam = achternaam;
        FunctieCode = functieCode;
    }
}