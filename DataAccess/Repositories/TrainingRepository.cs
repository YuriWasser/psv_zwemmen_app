using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class TrainingRepository(string connectionString, ILogger<TrainingRepository> logger) : ITrainingRepository
    {
        public List<Training> GetByGebruikerId(int gebruikerId)
        {
            try
            {
                List<Training> trainingen = new List<Training>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = @"
            SELECT t.id, t.zwembadId, t.datum, t.startTijd
            FROM training t
            WHERE NOT EXISTS (
                SELECT 1 FROM trainingAfmelden ta 
                WHERE ta.trainingId = t.id AND ta.gebruikerId = @gebruikerId
            )
            AND t.datum >= CURDATE()
            ORDER BY t.datum, t.startTijd
        ";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gebruikerId);

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    trainingen.Add(
                        new Training(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("zwembadId")),
                            reader.GetDateTime(reader.GetOrdinal("datum")),
                            reader.GetTimeSpan(reader.GetOrdinal("startTijd"))
                        )
                    );
                }

                return trainingen;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij ophalen van trainingen voor gebruiker.");
                throw new DatabaseException("Kon trainingen niet ophalen voor gebruiker.", ex);
            }
        }

        public Training GetById(int trainingId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, zwembadId, datum, startTijd FROM training WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", trainingId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Training(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("zwembadId")),
                        reader.GetDateTime(reader.GetOrdinal("datum")),
                        reader.GetTimeSpan(reader.GetOrdinal("startTijd"))
                    );
                }
                throw new TrainingNotFoundException($"Training met ID {trainingId} niet gevonden.");
            }
            catch (TrainingNotFoundException ex)
            {
                logger.LogError(ex, $"Training met ID {trainingId} niet gevonden.");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij ophalen van training met ID {trainingId}.");
                throw new DatabaseException($"Kon training met ID {trainingId} niet ophalen.", ex);
            }
        }

        public Training Add(Training training)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO training (zwembadId, datum, startTijd) VALUES (@zwembadId, @datum, @startTijd)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@zwembadId", training.ZwembadId);
                command.Parameters.AddWithValue("@datum", training.Datum);
                command.Parameters.AddWithValue("@startTijd", training.StartTijd);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Training(
                        newId, 
                        training.ZwembadId, 
                        training.Datum, 
                        training.StartTijd);
                }

                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij toevoegen van training.");
                throw new DatabaseException("Kon training niet toevoegen.", ex);
            }
        }

        public bool Update(Training training)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE training SET " +
                             "zwembadId = @zwembadId, " +
                             "datum = @datum, " +
                             "startTijd = @startTijd " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@zwembadId", training.ZwembadId);
                command.Parameters.AddWithValue("@datum", training.Datum);
                command.Parameters.AddWithValue("@startTijd", training.StartTijd);
                command.Parameters.AddWithValue("@id", training.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij updaten van training met ID {training.Id}.");
                throw new DatabaseException($"Kon training met ID {training.Id} niet bijwerken.", ex);
            }
        }

        public bool Delete(Training training)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM training WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", training.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij verwijderen van training met ID {training.Id}.");
                throw new DatabaseException($"Kon training met ID {training.Id} niet verwijderen.", ex);
            }
        }
    }
}