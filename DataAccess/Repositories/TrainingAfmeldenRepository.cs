using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class TrainingAfmeldenRepository : ITrainingAfmeldenRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public List<TrainingAfmelden> GetAll()
        {
            List<TrainingAfmelden> trainingAfmelden = new List<TrainingAfmelden>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM trainingAfmelden";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                trainingAfmelden.Add(
                    new TrainingAfmelden(
                        (int)reader["gebruikerId"],
                        (int)reader["trainingId"]
                    )
                );
            }

            return trainingAfmelden;
        }

        public TrainingAfmelden GetById(int trainingAfmeldenId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open(); 
            
            string sql = "SELECT * FROM trainingAfmelden WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", trainingAfmeldenId);
            
            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new TrainingAfmelden(
                    (int)reader["gebruikerId"],
                    (int)reader["trainingId"]
                );
            }

            return null;
        }

        public int Add(TrainingAfmelden trainingAfmelden)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO trainingAfmelden (gebruikerId, trainingId) VALUES (@gebruikerId, @trainingId)";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerId", trainingAfmelden.GebruikerId);
            command.Parameters.AddWithValue("@trainingId", trainingAfmelden.TrainingId);
            
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

        public bool Update(TrainingAfmelden trainingAfmelden)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE trainingAfmelden SET" +
                         "gebruikerId = @gebruikerId," +
                         "trainingId = @trainingId" +
                         "WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerId", trainingAfmelden.GebruikerId);
            command.Parameters.AddWithValue("@trainingId", trainingAfmelden.TrainingId);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(TrainingAfmelden trainingAfmelden)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM trainingAfmelden WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", trainingAfmelden.Id);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}