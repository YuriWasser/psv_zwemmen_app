using Core.Domain;
using Core.Interface;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class GebruikerRepository(string connectionString, ILogger<GebruikerRepository> logger) : IGebruikerRepository
    {
        public List<Gebruiker> GetAll()
        {
            try
            {
                List<Gebruiker> gebruikers = new List<Gebruiker>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT Id, Gebruikersnaam, Wachtwoord, Email, Voornaam, Achternaam, FunctieCode FROM gebruiker";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    gebruikers.Add(
                        new Gebruiker(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Gebruikersnaam")),
                            reader.GetString(reader.GetOrdinal("Wachtwoord")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("Voornaam")),
                            reader.GetString(reader.GetOrdinal("Achternaam")),
                            reader.GetString(reader.GetOrdinal("FunctieCode"))
                        )
                    );
                }

                return gebruikers;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij het ophalen van gebruikers.");
                throw new DatabaseException("Er is een databasefout opgetreden bij het ophalen van alle gebruikers.", ex);
            }
          
        }

        public Gebruiker GetById(int gebruikerId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "SELECT Id, Gebruikersnaam, Wachtwoord, Email, Voornaam, Achternaam, FunctieCode FROM gebruiker WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", gebruikerId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Gebruiker(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("Gebruikersnaam")),
                        reader.GetString(reader.GetOrdinal("Wachtwoord")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetString(reader.GetOrdinal("Voornaam")),
                        reader.GetString(reader.GetOrdinal("Achternaam")),
                        reader.GetString(reader.GetOrdinal("FunctieCode"))
                    );
                }

                throw new CompetitieNotFoundException("Geen gebruiker gevonden met ID");
            }
            catch (CompetitieNotFoundException ex)
            {
                logger.LogError(ex, "Geen gebruiker gevonden met ID");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij het ophalen van de gebruiker met ID");
                throw new DatabaseException("Er is een databasefout opgetreden bij het ophalen van de gebruiker met ID", ex);
            }
        }

        public Gebruiker Add(Gebruiker gebruiker)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO gebruiker " + 
                             "(gebruikersnaam, wachtwoord, email, voornaam, achternaam, functieCode) " + 
                             "VALUES (@gebruikersnaam, @wachtwoord, @email, @voornaam, @achternaam, @functieCode)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikersnaam", gebruiker.Gebruikersnaam);
                command.Parameters.AddWithValue("@wachtwoord", gebruiker.Wachtwoord);
                command.Parameters.AddWithValue("@email", gebruiker.Email);
                command.Parameters.AddWithValue("@voornaam", gebruiker.Voornaam);
                command.Parameters.AddWithValue("@achternaam", gebruiker.Achternaam);
                command.Parameters.AddWithValue("@functieCode", gebruiker.FunctieCode);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                { 
                    string selectId = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectId, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Gebruiker(
                        newId,
                        gebruiker.Gebruikersnaam,
                        gebruiker.Wachtwoord,
                        gebruiker.Email,
                        gebruiker.Voornaam,
                        gebruiker.Achternaam,
                        gebruiker.FunctieCode
                    );
                }
                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij het toevoegen van een gebruiker.");
                throw new DatabaseException("Er is een databasefout opgetreden bij het toevoegen van een gebruiker.", ex);
            }
        }

        public bool Update(Gebruiker gebruiker)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE gebruiker SET " +
                            "gebruikersnaam = @gebruikersnaam, " +
                            "wachtwoord = @wachtwoord, " +
                            "email = @email, " +
                            "voornaam = @voornaam, " +
                            "achternaam = @achternaam, " +
                            "functieCode = @functieCode " +
                            "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", gebruiker.Id);
                command.Parameters.AddWithValue("@gebruikersnaam", gebruiker.Gebruikersnaam);
                command.Parameters.AddWithValue("@wachtwoord", gebruiker.Wachtwoord);
                command.Parameters.AddWithValue("@email", gebruiker.Email);
                command.Parameters.AddWithValue("@voornaam", gebruiker.Voornaam);
                command.Parameters.AddWithValue("@achternaam", gebruiker.Achternaam);
                command.Parameters.AddWithValue("@functieCode", gebruiker.FunctieCode);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij het bijwerken van gebruiker met ID");
                throw new DatabaseException("Er is een databasefout opgetreden bij het bijwerken van gebruiker met ID", ex);
            }
        }

        public bool Delete(Gebruiker gebruiker)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM gebruiker WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", gebruiker.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij het verwijderen van gebruiker met ID");
                throw new DatabaseException($"Er is een databasefout opgetreden bij het verwijderen van gebruiker met ID", ex);
            }
        }
    }
}