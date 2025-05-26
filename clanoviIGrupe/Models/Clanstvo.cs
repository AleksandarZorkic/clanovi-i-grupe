using System;
using System.Collections.Generic;
using System.IO;


namespace clanoviIGrupe.Models
{
    public class Clanstvo
    {
        public int KorisnikId { get; set; }
        public int GrupaId { get; set; }

        public Clanstvo()
        {
        }

        public Clanstvo (int korisnikId, int grupaId)
        {
            KorisnikId = korisnikId;
            GrupaId = grupaId;
        }
    }
}
