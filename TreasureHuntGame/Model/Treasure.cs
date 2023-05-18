using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tresor.Model
{
    public class Treasure : MotionLessElement
    {
        public int NbTreasure { get; set; }
        public Treasure( int nbTreasure) : base('T')
        {
            NbTreasure = nbTreasure;
        
        }
    }
}

