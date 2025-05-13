using Core.Interface;
using Core.Domain;
using MySqlConnector;

namespace DataAccess.Repositories
{
    public class ProgrammaRepository : IProgrammaRepository
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        public List<Programma> GetAll()
        {
            List<Programma> programmas = new List<Programma>();

            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM programma";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                programmas.Add(
                    new Programma(
                        (int)reader["id"],
                        (int)reader["competitieId"],
                        (string)reader["omschrijving"],
                        (DateTime)reader["datum"],
                        (TimeSpan)reader["starttijd"]
                    )
                );
            }

            return programmas;
        }

        public Programma GetById(int programmaId)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM programma WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", programmaId);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Programma(
                    (int)reader["id"],
                    (int)reader["competitieId"],
                    (string)reader["omschrijving"],
                    (DateTime)reader["datum"],
                    (TimeSpan)reader["starttijd"]
                );
            }

            return null;
        }

        public int Add(Programma programma)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql =
                "INSERT INTO programma (competitieId, omschrijving, datum, starttijd) VALUES (@competitieId, @omschrijving, @datum, @starttijd)";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", programma.Id);
            command.Parameters.AddWithValue("@competitieId", programma.CompetitieId);
            command.Parameters.AddWithValue("@omschrijving", programma.Omschrijving);
            command.Parameters.AddWithValue("@datum", programma.Datum);
            command.Parameters.AddWithValue("starttijd", programma.StartTijd);

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

        public bool Update(Programma programma)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "UPDATE programma SET " +
                         "competitieId = @competitieId," +
                         "omschrijving = @omschrijving," +
                         "datum = @datum," +
                         "starttijd = @starttijd," +
                         "WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", programma.Id);
            command.Parameters.AddWithValue("@competitieId", programma.CompetitieId);
            command.Parameters.AddWithValue("@omschrijving", programma.Omschrijving);
            command.Parameters.AddWithValue("@datum", programma.Datum);
            command.Parameters.AddWithValue("starttijd", programma.StartTijd);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public bool Delete(Programma programma)
        {
            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = "DELETE FROM programma WHERE id = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", programma.Id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public List<Afstand> GetAfstandenByProgramma(int programmaId)
        {
            List<Afstand> afstanden = new List<Afstand>();

            using MySqlConnection connection = _dbConnection.GetConnection();
            connection.Open();

            string sql = @"
                SELECT a.id, a.meters, a.beschrijving
                FROM programma_afstand pa
                JOIN afstand a ON pa.afstandId = a.id
                WHERE pa.programmaId = @programmaId";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@programmaId", programmaId);

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
    }
}