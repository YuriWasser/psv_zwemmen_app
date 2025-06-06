using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Core.Service;

public class GebruikerService
{
    private readonly IGebruikerRepository _gebruikerRepository;
    private readonly ILogger<GebruikerService> _logger;
    private readonly IConfiguration _config;

    public GebruikerService(IGebruikerRepository gebruikerRepository, ILogger<GebruikerService> logger,
        IConfiguration config)
    {
        _gebruikerRepository = gebruikerRepository;
        _logger = logger;
        _config = config;
    }

    public List<Gebruiker> GetAll()
    {
        try
        {
            var result = _gebruikerRepository.GetAll();
            if (result == null)
            {
                throw new NullReferenceException("De repository retourneerde null");
            }

            return result;
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruikers");
            throw new DatabaseException("Er is een fout opgetreden bij het ophalen van gebruikers", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruikers");
            throw new Exception("Er is een fout opgetreden bij het ophalen van gebruikers", ex);
        }
    }

    public Gebruiker GetById(int id)
    {
        try
        {
            var gebruiker = _gebruikerRepository.GetById(id);
            if (gebruiker != null)
            {
                return gebruiker;
            }

            throw new Exception("Gebruiker niet gevonden");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruiker met ID {Id}", id);
            throw new Exception("Er is een fout opgetreden bij het ophalen van de gebruiker", ex);
        }
    }

    public Gebruiker Add(int id, string gebruikersnaam, string wachtwoord, string email, string voornaam,
        string achternaam, string functieCode)
    {
        try
        {
            _logger.LogInformation("Proberen nieuwe gebruiker toe te voegen: {Gebruikersnaam}, {Email}", gebruikersnaam,
                email);

            var newGebruiker = new Gebruiker(id, gebruikersnaam, wachtwoord, email, voornaam, achternaam, functieCode);
            var addedGebruiker = _gebruikerRepository.Add(newGebruiker);

            if (addedGebruiker == null)
            {
                _logger.LogWarning("Toevoegen van gebruiker is mislukt. Repository gaf null terug.");
                throw new Exception("Toevoegen van gebruiker is mislukt. Probeer het opnieuw.");
            }

            _logger.LogInformation("Gebruiker succesvol toegevoegd met ID: {Id}", addedGebruiker.Id);
            return addedGebruiker;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij toevoegen gebruiker: {Gebruikersnaam}", gebruikersnaam);
            throw new Exception("Er is een fout opgetreden bij het toevoegen van de gebruiker", ex);
        }
    }

    public bool Update(Gebruiker gebruiker)
    {
        try
        {
            return _gebruikerRepository.Update(gebruiker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij updaten gebruiker");
            throw new Exception("Er is een fout opgetreden bij het updaten van de gebruiker", ex);
        }
    }

    public bool Delete(int id)
    {
        try
        {
            var gebruiker = _gebruikerRepository.GetById(id);
            if (gebruiker == null)
            {
                throw new Exception("Gebruiker niet gevonden");
            }

            return _gebruikerRepository.Delete(gebruiker);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij verwijderen gebruiker");
            throw new Exception("Er is een fout opgetreden bij het verwijderen van de gebruiker", ex);
        }
    }

    public Gebruiker GetByGebruikersnaam(string gebruikersnaam)
    {
        try
        {
            var gebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruikersnaam);
            if (gebruiker != null)
            {
                return gebruiker;
            }

            throw new Exception("Gebruiker niet gevonden");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruiker met gebruikersnaam {Gebruikersnaam}", gebruikersnaam);
            throw new Exception("Er is een fout opgetreden bij het ophalen van de gebruiker", ex);
        }
    }

    public string Login(string gebruikersnaam, string wachtwoord)
    {
        try
        {
            var gebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruikersnaam);

            if (gebruiker == null)
            {
                throw new UnauthorizedAccessException("Ongeldige inloggegevens.");
            }

            var hasher = new PasswordHasher<Gebruiker>();

            var result = hasher.VerifyHashedPassword(gebruiker, gebruiker.Wachtwoord, wachtwoord);

            if (result != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Ongeldige inloggegevens.");
            }

            // Normaliseer functiecode naar rolnaam
            System.Console.WriteLine($"Functie code voor gebruiker {gebruikersnaam}: {gebruiker.Functie_Code}");

            string rol = NormalizeFunctieCode(gebruiker.Functie_Code);

            System.Console.WriteLine($"Rol voor gebruiker {gebruikersnaam}: {rol}");

            // JWT token maken
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, gebruiker.Id.ToString()),
                    new Claim(ClaimTypes.Name, gebruiker.Gebruikersnaam),
                    new Claim(ClaimTypes.Email, gebruiker.Email),
                    new Claim(ClaimTypes.Role, rol)
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["JwtSettings:ExpiresInMinutes"])),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij inloggen gebruiker {Gebruikersnaam}", gebruikersnaam);
            throw new Exception("Inloggen mislukt", ex);
        }
    }

    private string NormalizeFunctieCode(string functieCode)
    {
        if (string.IsNullOrWhiteSpace(functieCode))
            return "Gebruiker";

        System.Console.WriteLine(functieCode.ToUpper());

        return functieCode.ToUpper() switch
        {
            "TRAIN" => "Trainer", // Indien al goed
            "SWIM" => "Zwemmer",
            "EXSWIM" => "OudZwemmer",
            _ => "Gebruiker"
        };
    }

    public void Logout()
    {
        _logger.LogInformation("Gebruiker uitgelogd (JWT-token moet client-side verwijderd worden)");
    }
}