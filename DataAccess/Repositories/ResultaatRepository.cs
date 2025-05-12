using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class ResultaatRepository : IResultaatRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public List<Resultaat> GetAll()
        {
            List<Resultaat> resultaten = new List<Resultaat>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM resultaat";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                resultaten.Add(
                    new Resultaat(
                        (int)reader["id"],
                        (int)reader["gebruikerId"],
                        (int)reader["programmaId"],
                        (int)reader["afstandId"],
                        (TimeSpan)reader["tijd"],
                        (DateTime)reader["datum"]
                    )
                );
            }

            return resultaten;
        }

        public Resultaat GetById(int resultaatId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM resultaat WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", resultaatId);
            
            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Resultaat(
                    (int)reader["id"],
                    (int)reader["gebruikerId"],
                    (int)reader["programmaId"],
                    (int)reader["afstandId"],
                    (TimeSpan)reader["tijd"],
                    (DateTime)reader["datum"]
                );
            }

            return null;
        }

        public int Add(Resultaat resultaat)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO resultaat (gebruikerId, programmaId, afstandId, tijd, datum) VALUES (@gebruikerId, @programmaId, @AfstandId, @tijd, @datum)";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerId", resultaat.GebruikerId);
            command.Parameters.AddWithValue("@programmaId", resultaat.ProgrammaId);
            command.Parameters.AddWithValue("@afstandId", resultaat.AfstandId);
            command.Parameters.AddWithValue("@tijd", resultaat.Tijd);
            command.Parameters.AddWithValue("@datum", resultaat.Datum);
            
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

        public bool Update(Resultaat resultaat)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE resultaat SET" +
                         "gebruikerId = @gebruikerId," +
                         "programmaId = @programmaId," +
                         "afstandId = @afstandId," +
                         "tijd = @tijd," +
                         "datum = @datum" +
                         "WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerId", resultaat.GebruikerId);
            command.Parameters.AddWithValue("@programmaId", resultaat.ProgrammaId);
            command.Parameters.AddWithValue("@afstandId", resultaat.AfstandId);
            command.Parameters.AddWithValue("@tijd", resultaat.Tijd);
            command.Parameters.AddWithValue("@datum", resultaat.Datum);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Resultaat resultaat)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM resultaat WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", resultaat.Id);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}