using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class TrainingRepository(string connectionString, ILogger<TrainingRepository> logger) : ITrainingRepository
    {
        public List<Training> GetActieveTrainingen(int gebruikerId)
        {
            try
            {
                List<Training> trainingen = new List<Training>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = @"
            SELECT t.id, t.zwembad_id, t.datum, t.tijd
            FROM training t
            WHERE NOT EXISTS (
                SELECT 1 FROM gebruiker_training ta 
                WHERE ta.training_Id = t.id AND ta.gebruiker_id = @gebruikerId
            )
            AND t.datum >= CURDATE()
            ORDER BY t.datum, t.tijd
        ";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gebruikerId);

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    trainingen.Add(
                        new Training(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("zwembad_Id")),
                            reader.GetDateTime(reader.GetOrdinal("datum")),
                            reader.GetTimeSpan(reader.GetOrdinal("Tijd"))
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

                string sql = "SELECT id, zwembad_Id, datum, Tijd FROM training WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", trainingId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Training(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("zwembad_Id")),
                        reader.GetDateTime(reader.GetOrdinal("datum")),
                        reader.GetTimeSpan(reader.GetOrdinal("tijd"))
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

                string sql = "INSERT INTO training (zwembad_Id, datum, tijd) VALUES (@zwembadId, @datum, @tijd)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@zwembadId", training.ZwembadId);
                command.Parameters.AddWithValue("@datum", training.Datum);
                command.Parameters.AddWithValue("@tijd", training.Tijd);

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
                        training.Tijd);
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
                             "zwembad_Id = @zwembadId, " +
                             "datum = @datum, " +
                             "tijd = @tijd " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@zwembad_Id", training.ZwembadId);
                command.Parameters.AddWithValue("@datum", training.Datum);
                command.Parameters.AddWithValue("@tijd", training.Tijd);
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