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

                string sql = "SELECT id, gebruiker_id, programma_id, afstand_id, inschrijfdatum FROM inschrijving WHERE gebruikerId = @gebruikerId";

                using MySqlCommand command = new(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gebruikerId);

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inschrijvingen.Add(new WedstrijdInschrijving(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("gebruiker_Id")),
                        reader.GetInt32(reader.GetOrdinal("programma_Id")),
                        reader.GetInt32(reader.GetOrdinal("afstand_Id")),
                        reader.GetDateTime(reader.GetOrdinal("inschrijfDatum"))
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
                
                string sql = "SELECT id, gebruiker_id, programma_id, afstand_id, inschrijfdatum FROM inschrijving WHERE id = @id";
                
                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", wedstrijdInschrijvingId);
                
                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new WedstrijdInschrijving(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("gebruiker_Id")),
                        reader.GetInt32(reader.GetOrdinal("programma_Id")),
                        reader.GetInt32(reader.GetOrdinal("afstand_Id")),
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

                string sql = "INSERT INTO inschrijving (gebruiker_id, programma_id, afstand_id, inschrijfdatum) VALUES (@gebruikerId, @programmaId, @afstandId, @inschrijfDatum)";
                
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

                string sql = "UPDATE inschrijving SET " +
                             "gebruiker_id = @gebruikerId, " +
                             "programma_id = @programmaId, " +
                             "afstand_id = @afstandId, " +
                             "inschrijfdatum = @inschrijfDatum " +
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

                string sql = "DELETE FROM inschrijving WHERE id = @id";
                
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

        public bool Exists(int gebruikerId, int programmaId, int afstandId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "SELECT COUNT(*) FROM inschrijving WHERE gebruiker_id = @gebruikerId AND programma_id = @programmaId AND afstand_id = @afstandId";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gebruikerId);
                command.Parameters.AddWithValue("@programmaId", programmaId);
                command.Parameters.AddWithValue("@afstandId", afstandId);

                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij controleren van inschrijving bestaan.");
                throw new DatabaseException("Kon inschrijving niet controleren.", ex);
            }
        }

        public List<int> GetAfstandenByGebruikerEnProgramma(int gebruikerId, int programmaId)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "SELECT afstand_id FROM inschrijving WHERE gebruiker_id = @GebruikerId AND programma_id = @ProgrammaId";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@GebruikerId", gebruikerId);
                command.Parameters.AddWithValue("@ProgrammaId", programmaId);

                using var reader = command.ExecuteReader();

                var afstanden = new List<int>();
                while (reader.Read())
                {
                    afstanden.Add(reader.GetInt32(0)); 
                }
                return afstanden;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij ophalen van afstanden voor gebruiker en programma.");
                throw new DatabaseException("Kon afstanden niet ophalen.", ex);
            }
        }
    }
}