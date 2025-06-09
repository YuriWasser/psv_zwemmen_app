namespace Core.Dto;

public class LoginDto
{
    public int Id { get; set; }
    public string Gebruikersnaam { get; set; }
    public string Email { get; set; }
    public string Voornaam { get; set; }
    public string Achternaam { get; set; }
    public string FunctieCode { get; set; }
    public string TokenString { get; set; }
 
    
    public LoginDto(int id, string gebruikersnaam, string email, string voornaam, string achternaam, string functieCode, string tokenString)
    {
        Id = id;
        Gebruikersnaam = gebruikersnaam;
        Email = email;
        Voornaam = voornaam;
        Achternaam = achternaam;
        FunctieCode = functieCode;
        TokenString = tokenString;
    }
}