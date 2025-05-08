using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public List<Training> GetAll()
        {
            List<Training> trainingen = new List<Training>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM training";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                trainingen.Add(
                    new Training(
                        (int)reader["zwembadId"],
                        (DateTime)reader["datum"],
                        (TimeSpan)reader["startTijd"]
                    )
                );
            }

            return trainingen;
        }

        public Training GetById(int trainingId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM training WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", trainingId);
            
            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Training(
                    (int)reader["zwembadId"],
                    (DateTime)reader["datum"],
                    (TimeSpan)reader["startTijd"]
                );
            }

            return null;
        }

        public int Add(Training training)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
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
                using MySqlCommand selectIdCommand = new MySqlCommand(sql, connection);
                int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar());
                return newId;
            }

            return 0;
        }

        public bool Update(Training training)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE training SET" +
                         "zwembadId = @zwembadId," +
                         "datum = @datum," +
                         "startTijd = @startTijd" +
                         "WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@zwembadId", training.ZwembadId);
            command.Parameters.AddWithValue("@datum", training.Datum);
            command.Parameters.AddWithValue("@startTijd", training.StartTijd);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Training training)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM training WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", training.Id);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}