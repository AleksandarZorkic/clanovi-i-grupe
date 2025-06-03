using System.Xml.Linq;
using ClanstvoIGrupa_Dva.Models;
using Microsoft.Data.Sqlite;

namespace ClanstvoIGrupa_Dva.Repository
{
    public class UserDbRepository
    {
        private readonly string connectionString;
        public UserDbRepository(IConfiguration configuration) 
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
        public List<Korisnik> GetAll()
        {
            return ExecuteWithHandling(() =>
            {
                List<Korisnik> korisnici = new List<Korisnik>();
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string queryGet = "SELECT Id, Username, Name, Surname, Birthday FROM Users";
                using SqliteCommand command = new SqliteCommand(queryGet, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string korisnickoIme = reader["Username"].ToString();
                    string ime = reader["Name"].ToString();
                    string prezime = reader["Surname"].ToString();
                    DateTime rodjendan = DateTime.Parse(reader["Birthday"].ToString());

                    Korisnik korisnik = new Korisnik(id, korisnickoIme, ime, prezime, rodjendan);
                    korisnici.Add(korisnik);
                }
                return korisnici;
            });
        }

        public Korisnik? GetById(int id)
        {
            return ExecuteWithHandling(() =>
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = @"
                     SELECT Id, Username, Name, Surname, Birthday 
                     FROM Users 
                     WHERE Id = $Id";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["Id"]);
                    string username = reader["Username"].ToString();
                    string name = reader["Name"].ToString();
                    string surname = reader["Surname"].ToString();
                    DateTime birthday = DateTime.Parse(reader["Birthday"].ToString());

                    return new Korisnik(Id, username, name, surname, birthday);
                }
                return null;
            });
        }
        public Korisnik Create(string username, string name, string surname, string birthday)
        {
            return ExecuteWithHandling(() =>
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    INSERT INTO Users (Username, Name, Surname, Birthday)
                    VALUES (@Username, @Name, @Surname, @Birthday);
                    SELECT LAST_INSERT_ROWID()";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Surname", surname);
                DateTime dateOfbirth = DateTime.Parse(birthday);
                command.Parameters.AddWithValue("@Birthday", dateOfbirth.ToString("yyyy-MM-dd"));

                int lastInsertedId = Convert.ToInt32(command.ExecuteScalar());
                Korisnik noviKorisnik = new Korisnik(lastInsertedId, username, name, surname, dateOfbirth);

                return noviKorisnik;
            });
        } 
        public bool Update (Korisnik korisnik) 
        {
            return ExecuteWithHandling(() =>
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                    UPDATE Users
                    SET Username = @Username,
                    Name     = @Name,
                    Surname  = @Surname,
                    Birthday = @Birthday
                    WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Username", korisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Name", korisnik.Ime);
                command.Parameters.AddWithValue("@Surname", korisnik.Prezime);
                command.Parameters.AddWithValue("@Birthday", korisnik.DatumRodjenja.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Id", korisnik.Id);

                int promena = command.ExecuteNonQuery();

                return promena > 0;
            });
        }
        public bool Delete (Korisnik korisnik)
        {
            return ExecuteWithHandling(() =>
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", korisnik.Id);

                int removed = command.ExecuteNonQuery();
                return removed > 0;
            });
        }
        private T ExecuteWithHandling<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili neispravnom SQL upitu: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Neispravna operacija nad konekcijom: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }
    }
}
