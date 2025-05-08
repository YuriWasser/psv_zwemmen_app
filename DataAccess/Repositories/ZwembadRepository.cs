using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class ZwembadRepository : IZwembadRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();
        
        public List<Zwembad> GetAll()
        {
            List<Zwembad> zwembaden = new List<Zwembad>();

            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM zwembad";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                zwembaden.Add(
                    new Zwembad(
                        (string)reader["naam"],
                        (string)reader["adres"]
                    )
                );
            }

            return zwembaden;
        }

        public Zwembad GetById(int zwembadId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM zwembad WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", zwembadId);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Zwembad(
                    (string)reader["naam"],
                    (string)reader["adres"]
                );
                
            }

            return null;
        }

        public int Add(Zwembad zwembad)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO zwembad (naam, aders) VALUES (@naam, @adres)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@naam", zwembad.Naam);
            command.Parameters.AddWithValue("@adres", zwembad.Adres);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                string selectId = "SELECT LAST_INSERT_ID()";
                using MySqlCommand selectIdCommand = new MySqlCommand(sql, connection);
                int newId = Convert.ToInt32(selectIdCommand.ExecuteScalar);
                return newId;
            }

            return 0;
        }

        public bool Update(Zwembad zwembad)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE zwembad SET" +
                         "naam = @naam," +
                         "adres = @adres" +
                         "WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("naam", zwembad.Naam);
            command.Parameters.AddWithValue("adres", zwembad.Adres);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Zwembad zwembad)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM zwembad WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("id", zwembad.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}