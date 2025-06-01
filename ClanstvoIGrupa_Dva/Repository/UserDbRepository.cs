using ClanstvoIGrupa_Dva.Models;
using Microsoft.Data.Sqlite;

namespace ClanstvoIGrupa_Dva.Repository
{
    public class UserDbRepository
    {
        string putanja = "Data Source=database/mydatabase.db";
        public List<Korisnik> GetAll()
        {
            return ExecuteWithHandling(() =>
            {
                List<Korisnik> korisnici = new List<Korisnik>();
                using SqliteConnection connection = new SqliteConnection(putanja);
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
                using SqliteConnection connection = new SqliteConnection(putanja);
                connection.Open();
                string query = @"SELECT Id, Username, Name, Surname, Birthday 
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

        private T ExecuteWithHandling<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili neispravnom SQL upitu: {ex.Message}");
                return default!;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                return default!;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Neispravna operacija nad konekcijom: {ex.Message}");
                return default!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                return default!;
            }
        }

        private void ExecuteWithHandling(Action action)
        {
            try
            {
                action();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili neispravnom SQL upitu: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Neispravna operacija nad konekcijom: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
        }
    }
}
