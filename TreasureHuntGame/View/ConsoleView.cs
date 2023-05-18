using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureHunt.Model;

namespace TreasureHunt.View
{
    public class ConsoleView
    {
        public string GetMapContent(Map map, List<Adventurer> adventurers)
        {
            if (map != null)
            {
                StringBuilder mapContent = new StringBuilder();

                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        bool isAdventurer = adventurers.Any(adventurer => adventurer.X == x && adventurer.Y == y);
                        string finishLine = x + 1 == map.Width ? "" : " ";
                        if (isAdventurer)
                            mapContent.Append("A" + adventurers.Find(adventurer => adventurer.X == x && adventurer.Y == y).Name.ToLower().First() + finishLine);
                        else

                            mapContent.Append(map.MElements[x, y].Symbol + finishLine);
                    }
                    mapContent.AppendLine();
                }
                return mapContent.ToString();
            }
            else return "";
            
        }
    }
}
