using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class WedstrijdInschrijvingRepository : IWedstrijdInschrijvingRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public List<WedstrijdInschrijving> GetAll()
        {
            List<WedstrijdInschrijving> wedstrijdInschrijving = new List<WedstrijdInschrijving>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM wedstrijdInschrijving";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                wedstrijdInschrijving.Add(
                    new WedstrijdInschrijving(
                        (int)reader["gebruikerId"],
                        (int)reader["programmaId"],
                        (int)reader["afstandId"],
                        (DateTime)reader["inschrijfDatum"]
                    )
                );
            }

            return wedstrijdInschrijving;
        }

        public WedstrijdInschrijving GetById(int wedstrijdInschrijvingId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();
            
            string sql = "SELECT * FROM wedstrijdInschrijving WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", wedstrijdInschrijvingId);
            
            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new WedstrijdInschrijving(
                    (int)reader["gebruikerId"],
                    (int)reader["programmaId"],
                    (int)reader["afstandId"],
                    (DateTime)reader["inschrijfDatum"]
                );
            }

            return null;
        }

        public int Add(WedstrijdInschrijving wedstrijdInschrijving)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO wedstrijdinschrijving (gebruikerId, programmaId, afstandId, inschrijfDatum) VALUES (@gebruikerId, @programmaId, @afstandId, @inschrijfDatum)";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerId", wedstrijdInschrijving.GebruikerId);
            command.Parameters.AddWithValue("@programmaId", wedstrijdInschrijving.ProgrammaId);
            command.Parameters.AddWithValue("@afstandId", wedstrijdInschrijving.AfstandId);
            command.Parameters.AddWithValue("@inschrijfDatum", wedstrijdInschrijving.InschrijfDatum);
            
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

        public bool Update(WedstrijdInschrijving wedstrijdInschrijving)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE wedstrijdInschrijving SET" +
                         "gebruikerId = @gebruikerId," +
                         "programmaId = @programmaId," +
                         "afstandId = @afstandId," +
                         "inschrijfDatum = @inschrijfDatum" +
                         "WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@gebruikerId", wedstrijdInschrijving.GebruikerId);
            command.Parameters.AddWithValue("@programmaId", wedstrijdInschrijving.ProgrammaId);
            command.Parameters.AddWithValue("@afstandId", wedstrijdInschrijving.AfstandId);
            command.Parameters.AddWithValue("@inschrijfDatum", wedstrijdInschrijving.InschrijfDatum);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(WedstrijdInschrijving wedstrijdInschrijving)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM wedstrijdInschrijving WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", wedstrijdInschrijving.Id);
            
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
