using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class ClubrecordRepository : IClubrecordRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();
        
        public List<Clubrecord> GetAll()
        {
            List<Clubrecord> clubrecords = new List<Clubrecord>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM clubrecord";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                clubrecords.Add(
                    new Clubrecord(
                        (int)reader["id"],
                        (int)reader["gebruikerId"],
                        (int)reader["afstandId"],
                        (TimeSpan)reader["tijd"],
                        (DateTime)reader["datum"]
                    )
                );
            }

            return clubrecords;
        }

        public Clubrecord GetByID(int clubrecordId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT FROM clubrecords WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", clubrecordId);
            
            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Clubrecord(
                    (int)reader["id"],
                    (int)reader["gebruikerId"],
                    (int)reader["afstandId"],
                    (TimeSpan)reader["tijd"],
                    (DateTime)reader["datum"]
                );
            }

            return null;
        }

        public int Add(Clubrecord clubrecord)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO clubrecord (gebruikerId, afstandId, tijd, datum) VALUES (@gebruikerId, @afstandId, @tijd, @datum)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerid", clubrecord.GebruikerId);
            command.Parameters.AddWithValue("@afstandId", clubrecord.AfstandId);
            command.Parameters.AddWithValue("@tijd", clubrecord.Tijd);
            command.Parameters.AddWithValue("@datum", clubrecord.Datum);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                string selectIdSql = "SELECT LAST_INSERT_ID()";
                using MySqlCommand selectIdCommand = new MySqlCommand(sql, connection);
                int newId = Convert.ToInt32(selectIdCommand.ExecuteNonQuery());
                return newId;
            }

            return 0;
        }

        public bool Update(Clubrecord clubrecord)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE clubrecord SET" +
                         "gebruikerId = @gebruikerId," +
                         "afstandId = @afstandId," +
                         "tijd = @tijd," +
                         "datum = @datum," +
                         "WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerid", clubrecord.GebruikerId);
            command.Parameters.AddWithValue("@afstandId", clubrecord.AfstandId);
            command.Parameters.AddWithValue("@tijd", clubrecord.Tijd);
            command.Parameters.AddWithValue("@datum", clubrecord.Datum);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Clubrecord clubrecord)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM clubrecord WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", clubrecord.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}