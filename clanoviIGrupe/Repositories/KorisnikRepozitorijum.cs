using System.Security.Cryptography.X509Certificates;
using clanoviIGrupe.Models;

namespace clanoviIGrupe.Repositories
{
    public class KorisnikRepozitorijum
    {
        private const string filePath = "data/korisnici.csv";
        public static Dictionary<int, Korisnik> Data;

        public KorisnikRepozitorijum()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Korisnik>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int id = int.Parse(attributes[0]);
                string korisnickoIme = attributes[1];
                string ime = attributes[2];
                string prezime = attributes[3];
                DateTime datumRodjenja = DateTime.Parse(attributes[4]);
                Korisnik korisnik = new Korisnik(id, korisnickoIme, ime, prezime, datumRodjenja);
                Data[id] = korisnik;

            }
        }
        public void Save()
        {
            List<string> linije = new List<string>();
            foreach(Korisnik k in Data.Values)
            {
                linije.Add($"{k.Id},{k.KorisnickoIme},{k.Ime},{k.Prezime},{k.DatumRodjenja:yyyy-MM-dd}");
            }
            File.WriteAllLines(filePath, linije);
        }
    }
}
