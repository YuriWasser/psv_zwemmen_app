using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class TrainingAfmeldenRepository(string connectionString, ILogger<TrainingAfmeldenRepository> logger)
        : ITrainingAfmeldenRepository
    {
        public List<TrainingAfmelden> GetByGebruikerId(int gebruikerId)
        {
            try
            {
                List<TrainingAfmelden> trainingAfmelden = new List<TrainingAfmelden>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruikerId, trainingId FROM trainingAfmelden WHERE gebruikerId = @gebruikerId";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gebruikerId);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    trainingAfmelden.Add(
                        new TrainingAfmelden(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("gebruikerId")),
                            reader.GetInt32(reader.GetOrdinal("trainingId"))
                        )
                    );
                }

                return trainingAfmelden;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij ophalen van trainingafmeldingen voor gebruiker.");
                throw new DatabaseException("Kon trainingafmeldingen niet ophalen voor gebruiker.", ex);
            }
        }

        public TrainingAfmelden GetById(int trainingAfmeldenId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruikerId, trainingId FROM trainingAfmelden WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", trainingAfmeldenId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new TrainingAfmelden(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("gebruikerId")),
                        reader.GetInt32(reader.GetOrdinal("trainingId"))
                    );
                }
                throw new TrainingAfmeldenNotFoundException("TrainingAfmelden niet gevonden.");
            }
            catch (TrainingAfmeldenNotFoundException ex)
            {
                logger.LogError(ex,"TrainingAfmelden fetching by id");
                throw; 
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij ophalen van trainingAfmelden met ID {trainingAfmeldenId}.");
                throw new DatabaseException($"Kon trainingAfmelden met ID {trainingAfmeldenId} niet ophalen.", ex);
            }
        }

        public TrainingAfmelden Add(TrainingAfmelden trainingAfmelden)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "INSERT INTO trainingAfmelden (gebruikerId, trainingId) VALUES (@gebruikerId, @trainingId)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", trainingAfmelden.GebruikerId);
                command.Parameters.AddWithValue("@trainingId", trainingAfmelden.TrainingId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());

                    return new TrainingAfmelden(
                        newId, 
                        trainingAfmelden.GebruikerId, 
                        trainingAfmelden.TrainingId);
                }

                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij toevoegen van trainingAfmelden.");
                throw new DatabaseException("Kon trainingAfmelden niet toevoegen.", ex);
            }
        }

        public bool Update(TrainingAfmelden trainingAfmelden)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE trainingAfmelden SET " +
                             "gebruikerId = @gebruikerId, " +
                             "trainingId = @trainingId " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", trainingAfmelden.GebruikerId);
                command.Parameters.AddWithValue("@trainingId", trainingAfmelden.TrainingId);
                command.Parameters.AddWithValue("@id", trainingAfmelden.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij updaten van trainingAfmelden met ID {trainingAfmelden.Id}.");
                throw new DatabaseException($"Kon trainingAfmelden met ID {trainingAfmelden.Id} niet bijwerken.", ex);
            }
        }

        public bool Delete(TrainingAfmelden trainingAfmelden)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM trainingAfmelden WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", trainingAfmelden.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij verwijderen van trainingAfmelden met ID {trainingAfmelden.Id}.");
                throw new DatabaseException($"Kon trainingAfmelden met ID {trainingAfmelden.Id} niet verwijderen.", ex);
            }
        }
    }
}