using System.Xml.Linq;
using ClanstvoIGrupa_Dva.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace ClanstvoIGrupa_Dva.Repository
{
    public class PostDbRepository
    {
        private readonly string connectionString;
        private UserDbRepository _userRepo;

        public PostDbRepository(IConfiguration configuration, UserDbRepository userRepository)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
            _userRepo = userRepository;
        }

        public List<Post> GetAll(int page, int pageSize)
        {
            return ExecuteWithHandling(() =>
            {
                List<Post> postovi = new List<Post>();
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"
                        SELECT 
                        p.Id,
                        p.UserId,
                        p.Content,
                        p.Date,
                        u.Username,
                        u.Name,
                        u.Surname,
                        u.Birthday
                        FROM Posts p
                        LEFT JOIN Users u ON u.Id = p.UserId
                        ORDER BY p.Date DESC
                        LIMIT @pageSize OFFSET @offset;
                ";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@pageSize", page);
                command.Parameters.AddWithValue("@offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())

                {
                    int id = reader.GetInt32(0);
                    int? userId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
                    string content = reader.GetString(2);
                    DateTime date = DateTime.Parse(reader.GetString(3));

                    Korisnik? author = null;
                    if (userId.HasValue)
                    {
                        author = new Korisnik
                        {
                            Id = userId.Value,
                            KorisnickoIme = reader.GetString(4),
                            Ime = reader.GetString(5),
                            Prezime = reader.GetString(6),
                            DatumRodjenja = DateTime.Parse(reader.GetString(7))
                        };
                    }
                    
                    Post post = new Post
                    {
                        Id = id,
                        UserId = userId,
                        Content = content,
                        Date = date,
                        Author = author
                    };

                    postovi.Add(post);
                }
                return postovi;
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
