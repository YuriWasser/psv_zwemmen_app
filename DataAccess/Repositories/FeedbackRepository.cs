using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class FeedbackRepository(string connectionString, ILogger<FeedbackRepository> logger) : IFeedbackRepository
    {
        public List<Feedback> GetByZwemmerId(int gerbuikerId)
        {
            try
            {
                List<Feedback> feedbacks = new List<Feedback>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruiker_id, programma_id, feedback_text FROM feedback WHERE gebruiker_id = @gebruikerId";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", gerbuikerId);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    feedbacks.Add(
                        new Feedback(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("gebruiker_id")),
                            reader.GetInt32(reader.GetOrdinal("programma_id")),
                            reader.GetString(reader.GetOrdinal("feedback_text"))
                        )
                    );
                }

                return feedbacks;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij ophalen van feedback voor zwemmer met ID {gerbuikerId}");
                throw new DatabaseException($"Kon feedback niet ophalen voor zwemmer met ID ", ex);
            }
        }

        public Feedback GetById(int feedbackId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, gebruik_id, programma_id, feedback_text FROM feedback WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", feedbackId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Feedback(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("gebruik_id")),
                        reader.GetInt32(reader.GetOrdinal("programmaId")),
                        reader.GetString(reader.GetOrdinal("feedbackText"))
                    );
                }

                throw new FeedbackNotFoundException("Feedback niet gevonden");
            }
            catch (FeedbackNotFoundException)
            {
                logger.LogWarning($"Feedback met ID {feedbackId} niet gevonden");
                throw;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij ophalen van feedback met ID {feedbackId}");
                throw new DatabaseException($"Kon feedback met ID {feedbackId} niet ophalen", ex);
            }
        }

        public Feedback Add(Feedback feedback)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql =
                    "INSERT INTO feedback (gebruiker_id, programma_id, feedback_text) VALUES (@gebruikerId, @programmaId, @feedbackText)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", feedback.GebruikerId);
                command.Parameters.AddWithValue("@programmaId", feedback.ProgrammaId);
                command.Parameters.AddWithValue("@feedbackText", feedback.FeedbackText);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    string selectIdSql = "SELECT LAST_INSERT_ID()";
                    using MySqlCommand selectIdCommand = new MySqlCommand(selectIdSql, connection);
                    int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                    return new Feedback(
                        newId,
                        feedback.GebruikerId,
                        feedback.ProgrammaId,
                        feedback.FeedbackText
                    );
                }
                return null;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij toevoegen van feedback");
                throw new DatabaseException("Kon feedback niet toevoegen", ex);
            }
        }

        public bool Update(Feedback feedback)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "UPDATE feedback SET " +
                             "gebruiker_id = @zwemmerId, " + 
                             "programma_id = @programmaId, " +
                             "feedback_text = @feedbackText " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@gebruikerId", feedback.GebruikerId);
                command.Parameters.AddWithValue("@programmaId", feedback.ProgrammaId);
                command.Parameters.AddWithValue("@feedbackText", feedback.FeedbackText);
                command.Parameters.AddWithValue("@id", feedback.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij bijwerken van feedback met ID {feedback.Id}");
                throw new DatabaseException($"Kon feedback met ID {feedback.Id} niet bijwerken", ex);
            }
        }

        public bool Delete(Feedback feedback)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "DELETE FROM feedback WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", feedback.Id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, $"Fout bij verwijderen van feedback met ID {feedback.Id}");
                throw new DatabaseException($"Kon feedback met ID {feedback.Id} niet verwijderen", ex);
            }
        }
    }
}