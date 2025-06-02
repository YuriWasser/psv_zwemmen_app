namespace Presentation.ViewModels;

public class GebruikerViewModel
{
    public int Id { get; set; }
    public string Voornaam { get; set; }
    public string Achternaam { get; set; }
    public string Gebruikersnaam { get; set; }
    public string Email { get; set; }
    public string wachtwoord { get; set; }
    public string Functie { get; set; }
    
    public GebruikerViewModel(int id, string voornaam, string achternaam, string gebruikersnaam, string email, string wachtwoord, string functieCode)
    {
        Id = id;
        Voornaam = voornaam;
        Achternaam = achternaam;
        Gebruikersnaam = gebruikersnaam;
        Email = email;
        this.wachtwoord = wachtwoord;
        Functie = functieCode;
    }
}