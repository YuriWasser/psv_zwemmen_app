using Core.Domain;
using Core.Interface;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class AfstandRepository : IAfstandRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public List<Afstand> GetAll()
        {
            List<Afstand> afstanden = new List<Afstand>();

            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM afstand";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                afstanden.Add(
                    new Afstand(
                        (int)reader["id"],
                        (int)reader["meters"],
                        (string)reader["beschrijving"]
                    )
                );
            }

            return afstanden;
        }

        public Afstand GetByID(int afstandId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM afstand WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", afstandId);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Afstand(
                    (int)reader["id"],
                    (int)reader["meters"],
                    (string)reader["beschrijving"]
                );
            }

            return null;
        }

        public int Add(Afstand afstand)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "INSERT INTO afstand (meters, beschrijving) VALUES (@meters, @beschrijving)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@meters", afstand.Meters);
            command.Parameters.AddWithValue("@beschrijving", afstand.Beschrijving);
            

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

        public bool Update(Afstand afstand)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE afstand SET " +
                         "meters = @meters, " +
                         "beschrijving = @beschrijving " +
                         "WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@meters", afstand.Meters);
            command.Parameters.AddWithValue("@beschrijving", afstand.Beschrijving);
            command.Parameters.AddWithValue("@id", afstand.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Afstand afstand)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM afstand WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", afstand.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public List<Afstand> GetByProgrammaId(int ProgrammaId)
        {
            List<Afstand> afstanden = new List<Afstand>();

            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = @"
                SELECT a.id, a.meters, a.beschrijving
                FROM programma_afstand pa
                INNER JOIN afstand a ON pa.afstand_id = a.id
                WHERE pa.programma_id = @programmaId";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@programmaId", ProgrammaId);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                afstanden.Add(
                    new Afstand(
                        (int)reader["id"],
                        (int)reader["meters"],
                        (string)reader["beschrijving"]
                    ));
            }

            return null;
        }
    }
}