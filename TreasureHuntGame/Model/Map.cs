using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureHunt.Model;

namespace TreasureHunt.Model
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public MotionLessElement[,] MElements { get; set; }
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            MElements = new MotionLessElement[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    MElements[x, y] = new Plain();
                }
            }

        }

        

        public bool IsMountain(int x, int y)
        {
            if (IsValidPosition(x, y))
                return MElements[x,y] is Mountain;
            return false;
        }
        public bool IsAccessible(int x, int y)
        {
            if (IsValidPosition(x, y))
                return MElements[x, y] is Treasure || MElements[x, y] is Plain;
            return false;
        }

        public bool IsTreasure(int x, int y)
        {
            if (IsValidPosition(x, y))
                return MElements[x, y] is Treasure;
            return false;
        }

        public void AddMountain(int x, int y)
        {
            
            if (IsValidPosition(x,y) && MElements[x, y] is Plain)
                MElements[x, y] = new Mountain();
        }

        public void AddTreasure(int x, int y, int nbTreasure)
        {
            if (IsValidPosition(x,y) && MElements[x, y] is Plain)
                MElements[x, y] = new Treasure( nbTreasure);
        }

        public void RemoveTreasure(int x, int y)
        {
            var element = MElements[x,y];
            if (element is  Treasure treasure)
            {
                treasure.NbTreasure--;
                if (treasure.NbTreasure == 0)
                {
                    MElements[x, y] = new Plain();                }
            }
        
        }

        private bool IsValidPosition(int x, int y)
        {
           // Console.WriteLine(x + " " + y + " " + this.Width + " "+ this.Height);
            return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
        }

      

    }
}
