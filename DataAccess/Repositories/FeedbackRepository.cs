using Core.Domain;
using Core.Interface;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Core.Exceptions;

namespace DataAccess.Repositories
{
    public class FeedbackRepository(string connectionString, ILogger<FeedbackRepository> logger) : IFeedbackRepository
    {
        public List<Feedback> GetAll()
        {
            try
            {
                List<Feedback> feedbacks = new List<Feedback>();

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, zwemmerId, trainerId, programmaId, feedbackText FROM feedback";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    feedbacks.Add(
                        new Feedback(
                            reader.GetInt32(reader.GetOrdinal("id")),
                            reader.GetInt32(reader.GetOrdinal("zwemmerId")),
                            reader.GetInt32(reader.GetOrdinal("trainerId")),
                            reader.GetInt32(reader.GetOrdinal("programmaId")),
                            reader.GetString(reader.GetOrdinal("feedbackText"))
                        )
                    );
                }

                return feedbacks;
            }
            catch (MySqlException ex)
            {
                logger.LogError(ex, "Fout bij ophalen van alle feedback");
                throw new DatabaseException("Kon feedback niet ophalen", ex);
            }
        }

        public Feedback GetById(int feedbackId)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT id, zwemmerId, trainerId, programmaId, feedbackText FROM feedback WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", feedbackId);

                using MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Feedback(
                        reader.GetInt32(reader.GetOrdinal("id")),
                        reader.GetInt32(reader.GetOrdinal("zwemmerId")),
                        reader.GetInt32(reader.GetOrdinal("trainerId")),
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
                    "INSERT INTO feedback (zwemmerId, trainerId, programmaId, feedbackText) VALUES (@zwemmerId, @trainerId, @programmaId, @feedbackText)";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@zwemmerId", feedback.ZwemmerId);
                command.Parameters.AddWithValue("@trainerId", feedback.TrainerId);
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
                        feedback.ZwemmerId,
                        feedback.TrainerId,
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
                             "zwemmerId = @zwemmerId, " +
                             "trainerId = @trainerId, " +
                             "programmaId = @programmaId, " +
                             "feedbackText = @feedbackText " +
                             "WHERE id = @id";

                using MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@zwemmerId", feedback.ZwemmerId);
                command.Parameters.AddWithValue("@trainerId", feedback.TrainerId);
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