namespace ClanstvoIGrupa_Dva.Models
{
    public class Grupa
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateTime DatumOsnivanja { get; set; }

        public List<Korisnik> Korisnici { get; set; } = new();

        public Grupa()
        {
        }

        public Grupa(int id, string ime, DateTime datumOsnivanja)
        {
            Id = id;
            Ime = ime;
            DatumOsnivanja = datumOsnivanja;
        }
    }
}
