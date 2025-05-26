using System;
using System.Collections.Generic;
using System.IO;
using clanoviIGrupe.Models;

namespace clanoviIGrupe.Repositories
{
    public class GrupaRepozitorijum
    {
        private const string filePath = "data/grupe.csv";
        public static Dictionary<int, Grupa> Data;

        public GrupaRepozitorijum()
        {
            if (Data == null)
            {
                Load();
            }
        }

        private void Load()
        {
            Data = new Dictionary<int, Grupa>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] attribute = line.Split(',');
                int id = int.Parse(attribute[0]);
                string ime = attribute[1];
                DateTime datumOsnivanja = DateTime.Parse(attribute[2]);

                Grupa grupa = new Grupa(id, ime, datumOsnivanja);
                Data[id] = grupa;
            }
        }

        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (Grupa g in Data.Values)
            {
                lines.Add($"{g.Id},{g.Ime},{g.DatumOsnivanja:yyyy-MM-dd}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
