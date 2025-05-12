using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();
        public List<Feedback> GetAll()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM feedback";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                feedbacks.Add(
                    new Feedback(
                        (int)reader["id"],
                        (int)reader["zwemmerId"],
                        (int)reader["trainerId"],
                        (int)reader["programmaId"],
                        (string)reader["feedbackText"]
                    )
                );
            }

            return feedbacks;
        }

        public Feedback GetById(int feedbackId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM feedback WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", feedbackId);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Feedback(
                    (int)reader["id"],
                    (int)reader["zwemmerId"],
                    (int)reader["trainerId"],
                    (int)reader["programmaId"],
                    (string)reader["feedbackText"]
                );
            }

            return null;
        }

        public int Add(Feedback feedback)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO feedback (zwemmerId, trainerId, programmaId, feedbackText) VALUE (@zwemmerId, @trainerId, @programmaId, @feedbackText)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@zwemmerId", feedback.ZwemmerId);
            command.Parameters.AddWithValue("@trainerId", feedback.TrainerId);
            command.Parameters.AddWithValue("@programmaId", feedback.ProgrammaId);
            command.Parameters.AddWithValue("@feedbackText", feedback.FeedbackText);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                string selectIdSql = "SELECT LAST_INSERT_ID()";
                using MySqlCommand selectIdCommand = new MySqlCommand(sql, connection);
                int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                return newId;
            }

            return 0;
        }

        public bool Update(Feedback feedback)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE feedback SET" +
                         "zwemmerId = @zwemmerId," +
                         "trainerId = @trainerId," +
                         "programmaId = @programmaId," +
                         "feedbackText = @feedbackText," +
                         "WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@zwemmerId", feedback.ZwemmerId);
            command.Parameters.AddWithValue("@trainerId", feedback.TrainerId);
            command.Parameters.AddWithValue("@programmaId", feedback.ProgrammaId);
            command.Parameters.AddWithValue("@feedbackText", feedback.FeedbackText);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;

        }

        public bool Delete(Feedback feedback)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM feedback WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", feedback.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}