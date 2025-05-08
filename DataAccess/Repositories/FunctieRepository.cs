using Core.Domain;
using Core.Interface;
using MySqlConnector;
namespace DataAccess.Repositories
{
    public class FunctieRepository : IFunctieRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public List<Functie> GetAll()
        {
            List<Functie> functies = new List<Functie>();
            
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM functie";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                functies.Add(
                    new Functie(
                        (string)reader["code"],
                        (string)reader["beschrijving"]
                    )
                ); 
            }

            return functies;
        }

        public Functie GetById(int functieId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECTED * FROM functie WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", functieId);

            using MySqlDataReader reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return new Functie(
                    (string)reader["code"],
                    (string)reader["beschrijving"]
                );
            }

            return null;
        }

        public int Add(Functie functie)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "ISNERT INTO functie (code, beschrijving) VALUES (@code, @beschrijving)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@code", functie.Code);
            command.Parameters.AddWithValue("@beschrijving", functie.Beschrijving);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                string selectIdSql = "SELECT LAST_INSERT_ID()";
                using MySqlCommand selectIdCommand = new MySqlCommand(sql, connection);
                int newId= Convert.ToInt32(selectIdCommand.ExecuteScalar());
                return newId;
            }

            return 0;
        }

        public bool Update(Functie functie)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE functie SET" +
                         "code = @code," +
                         "beschrijving = @beschrijving," +
                         "WHERE id = @id";
            
            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@code", functie.Code);
            command.Parameters.AddWithValue("@beschrijving", functie.Beschrijving);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Functie functie)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM functie WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", functie.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}