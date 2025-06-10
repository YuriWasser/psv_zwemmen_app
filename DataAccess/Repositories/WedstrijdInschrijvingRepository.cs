using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class WedstrijdInschrijvingRepository(string connectionString, ILogger<WedstrijdInschrijvingRepository> logger) : IWedstrijdInschrijvingRepository
    {
        public List<WedstrijdInschrijving> GetByGebruikerId(int gebruikerId)
        {
            try
            {
                List<WedstrijdInschrijving> inschrijvingen = new();

                using MySqlConnection connection = new(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruikerId, programmaId, afstandId, inschrijfDatum FROM wedstrijdInschrijving WHERE gebruikerId = @gebruikerId";

                using MySqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gebruikerId);

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inschrijvingen.Add(new WedstrijdInschrijving(
                        reader.GetInt32("id"),
                        reader.GetInt32("gebruikerId"),
                        reader.GetInt32("programmaId"),
                        reader.GetInt32("afstandId"),
                        reader.GetDateTime("inschrijfDatum")
                    ));
                }

                return inschrijvingen;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij ophalen van inschrijvingen van gebruiker.");
                throw new DatabaseException("Kon inschrijvingen van gebruiker niet ophalen.", ex);
            }
        }

        public WedstrijdInschrijving GetById(int wedstrijdInschrijvingId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                
                string sql = "SELECT id, gebruikerId, programmaId, afstandId, inschrijfDatum FROM wedstrijdInschrijving WHERE id = @id";
                
                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", wedstrijdInschrijvingId);
                
                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new WedstrijdInschrijving(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("gebruikerId")),
                        reader.GetInt32(reader.GetOrdinal("programmaId")),
                        reader.GetInt32(reader.GetOrdinal("afstandId")),
                        reader.GetDateTime(reader.GetOrdinal("inschrijfDatum"))
                    );
                }

                throw new WedstrijdInschrijvingNotFoundException($"Wedstrijd inschrijving met ID {wedstrijdInschrijvingId} niet gevonden.");
            }
            catch (WedstrijdInschrijvingNotFoundException ex)
            {
                logger.LogWarning(ex, $"Wedstrijd inschrijving met ID {wedstrijdInschrijvingId} niet gevonden.");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij ophalen van inschrijving met ID {wedstrijdInschrijvingId}.");
                throw new DatabaseException("Fout bij ophalen van de inschrijving.", ex);
            }
        }

        public WedstrijdInschrijving Add(WedstrijdInschrijving wedstrijdInschrijving)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO wedstrijdinschrijving (gebruikerId, programmaId, afstandId, inschrijfDatum) VALUES (@gebruikerId, @programmaId, @afstandId, @inschrijfDatum)";
                
                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", wedstrijdInschrijving.GebruikerId);
                command.Parameters.AddWithValue("@programmaId", wedstrijdInschrijving.ProgrammaId);
                command.Parameters.AddWithValue("@afstandId", wedstrijdInschrijving.AfstandId);
                command.Parameters.AddWithValue("@inschrijfDatum", wedstrijdInschrijving.InschrijfDatum);
                
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());

                    return new WedstrijdInschrijving(
                        newId,
                        wedstrijdInschrijving.GebruikerId,
                        wedstrijdInschrijving.ProgrammaId,
                        wedstrijdInschrijving.AfstandId,
                        wedstrijdInschrijving.InschrijfDatum
                    );
                }

                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij toevoegen van wedstrijdinschrijving.");
                throw new DatabaseException("Kon wedstrijdinschrijving niet toevoegen.", ex);
            }
        }

        public bool Update(WedstrijdInschrijving wedstrijdInschrijving)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE wedstrijdInschrijving SET " +
                             "gebruikerId = @gebruikerId, " +
                             "programmaId = @programmaId, " +
                             "afstandId = @afstandId, " +
                             "inschrijfDatum = @inschrijfDatum " +
                             "WHERE id = @id";
                
                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", wedstrijdInschrijving.GebruikerId);
                command.Parameters.AddWithValue("@programmaId", wedstrijdInschrijving.ProgrammaId);
                command.Parameters.AddWithValue("@afstandId", wedstrijdInschrijving.AfstandId);
                command.Parameters.AddWithValue("@inschrijfDatum", wedstrijdInschrijving.InschrijfDatum);
                command.Parameters.AddWithValue("@id", wedstrijdInschrijving.Id);
                
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij updaten van inschrijving met ID {wedstrijdInschrijving.Id}.");
                throw new DatabaseException("Kon inschrijving niet bijwerken.", ex);
            }
        }

        public bool Delete(WedstrijdInschrijving wedstrijdInschrijving)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM wedstrijdInschrijving WHERE id = @id";
                
                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", wedstrijdInschrijving.Id);
                
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij verwijderen van inschrijving met ID {wedstrijdInschrijving.Id}.");
                throw new DatabaseException("Kon inschrijving niet verwijderen.", ex);
            }
        }
    }
}