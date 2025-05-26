using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using clanoviIGrupe.Models;

namespace clanoviIGrupe.Repositories
{
    public class ClanstvoRepozitorijum
    {
        private const string filePath = "data/clanstva.csv";
        public static List<Clanstvo> Data;

        public ClanstvoRepozitorijum()
        {
            if (Data == null)
            {
                Load();
            }
        }

        public void Load()
        {
            Data = new List<Clanstvo>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attributes = line.Split(',');
                int korisnikId = int.Parse(attributes[0]);
                int groupId = int.Parse(attributes[1]);
                Clanstvo clanstvo = new Clanstvo(korisnikId, groupId);
                Data.Add(clanstvo);
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Clanstvo c in Data)
            {
                lines.Add($"{c.KorisnikId},{c.GrupaId}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
