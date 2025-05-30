using System;
using System.Collections.Generic;
using clanoviIGrupe.Models;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Data.Sqlite;


namespace clanoviIGrupe.Repositories
{
    public class KorisnikRepozitorijum
    {
        string putanja = "Data Source=database/mydatabase.db";
        public List<Korisnik> GetFromDataBase()
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
        }

        public Korisnik AddToDatabase (Korisnik k)
        {
            using SqliteConnection conncetion = new SqliteConnection(putanja);
            conncetion.Open();

            string queryAdd = @"
                INSERT INTO Users (Username, Name, Surname, Birthday) 
                VALUES ($u, $n, $s, $b);
            ";
            SqliteCommand command = new SqliteCommand(queryAdd, conncetion);
            command.Parameters.AddWithValue("$u", k.KorisnickoIme);
            command.Parameters.AddWithValue("$n", k.Ime);
            command.Parameters.AddWithValue("$s", k.Prezime);
            command.Parameters.AddWithValue("$b", k.DatumRodjenja.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();

            using var idCmd = conncetion.CreateCommand();
            idCmd.CommandText = "SELECT last_insert_rowid();";
            long newId = (long)idCmd.ExecuteScalar();
            k.Id = (int)newId;

            return k;
        }

        public Korisnik? UpdateInDatabase (int id, Korisnik k)
        {
            using SqliteConnection connection = new SqliteConnection(putanja);
            connection.Open();

            string queryUpdate = @"
                UPDATE Users 
                SET 
                    Username = $u, 
                    Name = $n, 
                    Surname = $s, 
                    Birthday = $b 
                WHERE Id = $i;
            ";
            using SqliteCommand command = new SqliteCommand(queryUpdate, connection);
            command.Parameters.AddWithValue("$u", k.KorisnickoIme);
            command.Parameters.AddWithValue("$n", k.Ime);
            command.Parameters.AddWithValue("$s", k.Prezime);
            command.Parameters.AddWithValue("$b", k.DatumRodjenja.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("$i", id);

            int promena = command.ExecuteNonQuery();
            if (promena == 0)
                return null;

            k.Id = id;
            return k;
        }
    }
}
