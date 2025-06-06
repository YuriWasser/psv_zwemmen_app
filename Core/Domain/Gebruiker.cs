using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;
public class Gebruiker
{
    public int Id { get; private set; }
    public string Gebruikersnaam { get; private set; } 
    public string Wachtwoord { get; private set; }
    public string Email { get; private set; } 
    public string Voornaam { get; private set; }
    public string Achternaam { get; private set; }
    public string Functie_Code { get; private set; }


    public Gebruiker(int id, string gebruikersnaam, string wachtwoord, string email, string voornaam, string achternaam,
        string functieCode)
    {
        Id = id;
        Gebruikersnaam = gebruikersnaam;
        Wachtwoord = wachtwoord;
        Email = email;
        Voornaam = voornaam;
        Achternaam = achternaam;
        Functie_Code = functieCode;
    }
}