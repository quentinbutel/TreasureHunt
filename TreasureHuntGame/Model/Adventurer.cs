using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace TreasureHunt.Model
{
    public class Adventurer
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int NbTreasureTaken { get; set; }
        public string Name { get; set; }
        public Orientation Orientation { get; set; }
        public List<Movement> MovementsSequence { get; set; }


        public Adventurer(string name,  int x, int y,  Orientation orientation, string movements)
        {
            X = x;
            Y = y;
            Name = name;
            Orientation = orientation;
            NbTreasureTaken = 0;
            MovementsSequence = new List<Movement>();
            movements.ToCharArray().ToList().ForEach(m => MovementsSequence.Add((Movement)Enum.Parse(typeof(Movement), m.ToString())));
        }


        
   

        
    }
}
