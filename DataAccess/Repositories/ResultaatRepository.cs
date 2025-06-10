using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class ResultaatRepository(string connectionString, ILogger<ResultaatRepository> logger)
        : IResultaatRepository
    {
        public List<Resultaat> GetByGebruikerId(int gebruikerId)
        {
            try
            {
                List<Resultaat> resultaten = new List<Resultaat>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruikerId, programmaId, afstandId, tijd, datum FROM resultaat WHERE gebruikerId = @gebruikerId";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gebruikerId);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    resultaten.Add(
                        new Resultaat(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("gebruikerId")),
                            reader.GetInt32(reader.GetOrdinal("programmaId")),
                            reader.GetInt32(reader.GetOrdinal("afstandId")),
                            reader.GetTimeSpan(reader.GetOrdinal("tijd")),
                            reader.GetDateTime(reader.GetOrdinal("datum"))
                        )
                    );
                }

                return resultaten;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij ophalen van resultaten voor gebruiker.");
                throw new DatabaseException("Kon resultaten niet ophalen voor gebruiker.", ex);
            }
        }

        public Resultaat GetById(int resultaatId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruikerId, programmaId, afstandId, tijd, datum FROM resultaat WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", resultaatId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Resultaat(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("gebruikerId")),
                        reader.GetInt32(reader.GetOrdinal("programmaId")),
                        reader.GetInt32(reader.GetOrdinal("afstandId")),
                        reader.GetTimeSpan(reader.GetOrdinal("tijd")),
                        reader.GetDateTime(reader.GetOrdinal("datum"))
                    );
                }

                throw new ResultaatNotFoundException("Resultaat niet gevonden.");
            }
            catch (ResultaatNotFoundException ex)
            {
                logger.LogError(ex, $"Resultaat met ID {resultaatId} niet gevonden.");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij ophalen van resultaat met ID {resultaatId}.");
                throw new DatabaseException($"Kon resultaat met ID {resultaatId} niet ophalen.", ex);
            }
        }

        public Resultaat Add(Resultaat resultaat)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "INSERT INTO resultaat (gebruikerId, programmaId, afstandId, tijd, datum) " +
                    "VALUES (@gebruikerId, @programmaId, @afstandId, @tijd, @datum)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", resultaat.GebruikerId);
                command.Parameters.AddWithValue("@programmaId", resultaat.ProgrammaId);
                command.Parameters.AddWithValue("@afstandId", resultaat.AfstandId);
                command.Parameters.AddWithValue("@tijd", resultaat.Tijd);
                command.Parameters.AddWithValue("@datum", resultaat.Datum);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Resultaat(
                        newId, 
                        resultaat.GebruikerId, 
                        resultaat.ProgrammaId, 
                        resultaat.AfstandId, 
                        resultaat.Tijd, 
                        resultaat.Datum);
                }

                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij toevoegen van resultaat.");
                throw new DatabaseException("Kon resultaat niet toevoegen.", ex);
            }
        }

        public bool Update(Resultaat resultaat)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE resultaat SET " +
                             "gebruikerId = @gebruikerId, " +
                             "programmaId = @programmaId, " +
                             "afstandId = @afstandId, " +
                             "tijd = @tijd, " +
                             "datum = @datum " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", resultaat.GebruikerId);
                command.Parameters.AddWithValue("@programmaId", resultaat.ProgrammaId);
                command.Parameters.AddWithValue("@afstandId", resultaat.AfstandId);
                command.Parameters.AddWithValue("@tijd", resultaat.Tijd);
                command.Parameters.AddWithValue("@datum", resultaat.Datum);
                command.Parameters.AddWithValue("@id", resultaat.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij bijwerken van resultaat met ID {resultaat.Id}.");
                throw new DatabaseException($"Kon resultaat met ID {resultaat.Id} niet bijwerken.", ex);
            }
        }

        public bool Delete(Resultaat resultaat)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM resultaat WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", resultaat.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij verwijderen van resultaat met ID {resultaat.Id}.");
                throw new DatabaseException($"Kon resultaat met ID {resultaat.Id} niet verwijderen.", ex);
            }
        }
    }
}