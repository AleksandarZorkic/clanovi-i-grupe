namespace ClanstvoIGrupa_Dva.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public DateTime Date {  get; set; }
        public Korisnik? Author { get; set; }

    }
}
