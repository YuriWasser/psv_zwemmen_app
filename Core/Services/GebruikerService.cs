using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Dto;

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
            if (string.IsNullOrWhiteSpace(gebruikersnaam))
                throw new ArgumentException("Gebruikersnaam mag niet leeg zijn", nameof(gebruikersnaam));
            if (string.IsNullOrWhiteSpace(wachtwoord))
                throw new ArgumentException("Wachtwoord mag niet leeg zijn", nameof(wachtwoord));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email mag niet leeg zijn", nameof(email));
            if (string.IsNullOrWhiteSpace(voornaam))
                throw new ArgumentException("Voornaam mag niet leeg zijn", nameof(voornaam));
            if (string.IsNullOrWhiteSpace(achternaam))
                throw new ArgumentException("Achternaam mag niet leeg zijn", nameof(achternaam));
            if (string.IsNullOrWhiteSpace(functieCode))
                throw new ArgumentException("FunctieCode mag niet leeg zijn", nameof(functieCode));
            
            _logger.LogInformation("Proberen nieuwe gebruiker toe te voegen: {Gebruikersnaam}, {Email}", gebruikersnaam,
                email);

            var hashedPassword = PasswordHasher.HashPassword(wachtwoord);

            var newGebruiker = new Gebruiker(id, gebruikersnaam, hashedPassword, email, voornaam, achternaam,
                functieCode);
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
            if (string.IsNullOrWhiteSpace(gebruikersnaam))
                throw new ArgumentException("Gebruikersnaam mag niet leeg zijn", nameof(gebruikersnaam));
            
            var gebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruikersnaam);
            if (gebruiker == null)
            {
                throw new Exception("Gebruiker niet gevonden");
            }

            return gebruiker;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij ophalen gebruiker met gebruikersnaam {Gebruikersnaam}", gebruikersnaam);
            throw new Exception("Er is een fout opgetreden bij het ophalen van de gebruiker", ex);
        }
    }

    public LoginDto Login(string gebruikersnaam, string wachtwoord, string jwtKey, string jwtIssuer)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(gebruikersnaam))
                throw new InvalidDataException($"Gebruikersnaam mag niet leeg zijn: {gebruikersnaam}");

            if (string.IsNullOrWhiteSpace(wachtwoord))
                throw new InvalidDataException($"Wachtwoord mag niet leeg zijn: {wachtwoord}");

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidDataException($"JWT Key mag niet leeg zijn: {jwtKey}");

            if (string.IsNullOrWhiteSpace(jwtIssuer))
                throw new InvalidDataException($"JWT Issuer mag niet leeg zijn: {jwtIssuer}");

            Gebruiker? gebruiker = _gebruikerRepository.GetByGebruikersnaam(gebruikersnaam);

            if (gebruiker == null)
                throw new GebruikerNotFoundException("Ongeldige gebruikersnaam of wachtwoord");

            if (!PasswordHasher.VerifyPassword(gebruiker.Wachtwoord, wachtwoord))
                throw new GebruikerNotFoundException("Ongeldige wachtwoord");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, gebruiker.Gebruikersnaam),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("GebruikerId", gebruiker.Id.ToString()),
                new Claim("Gebruikersnaam", gebruiker.Gebruikersnaam),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            LoginDto dto = new LoginDto(
                gebruiker.Id,
                gebruiker.Gebruikersnaam,
                gebruiker.Email,
                gebruiker.Voornaam,
                gebruiker.Achternaam,
                NormalizeFunctieCode(gebruiker.Functie_Code),
                tokenString
            );
            return dto;
        }
        catch (GebruikerNotFoundException ex)
        {
            _logger.LogWarning(ex, $"Login mislukt voor gebruiker: {gebruikersnaam}");
            throw;
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex, $"Ongeldige argumenten bij inloggen gebruiker: {gebruikersnaam}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Fout bij inloggen gebruiker: {gebruikersnaam}");
            throw;
        }
    }

    public string NormalizeFunctieCode(string functieCode)
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