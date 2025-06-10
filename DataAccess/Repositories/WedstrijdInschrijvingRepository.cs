using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class WedstrijdInschrijvingRepository(string connectionString, ILogger<WedstrijdInschrijvingRepository> logger) : IWedstrijdInschrijvingRepository
    {
        public List<WedstrijdInschrijving> GetAll()
        {
            try
            {
                List<WedstrijdInschrijving> wedstrijdInschrijving = new List<WedstrijdInschrijving>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "SELECT id, gebruikerId, programmaId, afstandId, inschrijfDatum FROM wedstrijdInschrijving";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    wedstrijdInschrijving.Add(
                        new WedstrijdInschrijving(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("gebruikerId")),
                            reader.GetInt32(reader.GetOrdinal("programmaId")),
                            reader.GetInt32(reader.GetOrdinal("afstandId")),
                            reader.GetDateTime(reader.GetOrdinal("inschrijfDatum"))
                        )
                    );
                }

                return wedstrijdInschrijving;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Database error occurred while retrieving wedstrijd inschrijvingen.");
                throw new DatabaseException("Er is een fout opgetreden bij het ophalen van wedstrijd inschrijvingen.", ex);
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