using Core.Interface;
using Core.Domain;
using Core.Exceptions;
using MySqlConnector;
using Microsoft.Extensions.Logging;

namespace DataAccess.Repositories
{
    public class ProgrammaRepository(string connectionString, ILogger<ProgrammaRepository> logger) : IProgrammaRepository
    {
        public Programma GetById(int programmaId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, competitie_id, omschrijving, datum, start_tijd FROM programma WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", programmaId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Programma(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("competitie_id")),
                        reader.GetString(reader.GetOrdinal("omschrijving")),
                        reader.GetDateTime(reader.GetOrdinal("datum")),
                        reader.GetTimeSpan(reader.GetOrdinal("start_tijd"))
                    );
                }

                throw new ProgrammaNotFoundException($"Programma with id {programmaId} not found");
            }
            catch (ProgrammaNotFoundException ex)
            {
                logger.LogError(ex, "Programma not found");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error fetching programma by id");
                throw new DatabaseException("Error fetching programma by id", ex);
            }
        }

        public Programma Add(Programma programma)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "INSERT INTO programma (competitie_id, omschrijving, datum, start_tijd) VALUES (@competitie_id, @omschrijving, @datum, @start_tijd)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@competitie_id", programma.CompetitieId);
                command.Parameters.AddWithValue("@omschrijving", programma.Omschrijving);
                command.Parameters.AddWithValue("@datum", programma.Datum);
                command.Parameters.AddWithValue("@start_tijd", programma.StartTijd);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Programma(
                        newId,
                        programma.CompetitieId,
                        programma.Omschrijving,
                        programma.Datum,
                        programma.StartTijd
                    );
                }
                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error adding programma");
                throw new DatabaseException("Error adding programma", ex);
            }
        }

        public bool Update(Programma programma)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "UPDATE programma SET competitie_id = @competitie_id, omschrijving = @omschrijving, datum = @datum, start_tijd = @start_tijd WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", programma.Id);
                command.Parameters.AddWithValue("@competitie_id", programma.CompetitieId);
                command.Parameters.AddWithValue("@omschrijving", programma.Omschrijving);
                command.Parameters.AddWithValue("@datum", programma.Datum);
                command.Parameters.AddWithValue("@start_tijd", programma.StartTijd);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error updating programma");
                throw new DatabaseException("Error updating programma", ex);
            }
        }

        public bool Delete(Programma programma)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM programma WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", programma.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Error deleting programma");
                throw new DatabaseException("Error deleting programma", ex);
            }
        }
        
        public List<Programma> GetByCompetitieId(int competitie_Id)
        {
            var result = new List<Programma>();

            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, competitie_id, omschrijving, datum, start_tijd FROM programma WHERE competitie_id = @competitie_id";

                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@competitie_id", competitie_Id);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Programma(
                        reader.GetInt32("id"),
                        reader.GetInt32("competitie_id"),
                        reader.GetString("omschrijving"),
                        reader.GetDateTime("datum"),
                        reader.GetTimeSpan("start_tijd")
                    ));
                }

                return result;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij ophalen programma's voor competitie");
                throw new DatabaseException("Fout bij ophalen programma's voor competitie", ex);
            }
        }

    }
}