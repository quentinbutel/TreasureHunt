using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureHunt.Controller;
using TreasureHunt.Model;
using FluentAssertions;
using TreasureHunt.View;

namespace TreasureHunt.UnitTests
{
    [TestClass]
    public class ConsoleViewTest
    {

        [TestMethod]
        public void GetMapContent_DesignWithMap_Mountain_Treasure_Adventurer()
        {

            Map map = new Map(3,3);
            map.AddMountain(1, 1);
            map.AddTreasure(2, 2, 2);
            List<Adventurer> adventurers = new List<Adventurer>();
            adventurers.Add(new Adventurer("indiana", 1, 2, Orientation.N, "AADADA"));
            StringBuilder mapContent = new StringBuilder();

            mapContent.AppendLine(". . .");
            mapContent.AppendLine(". M .");
            mapContent.AppendLine(". Ai T");

            ConsoleView consoleView = new ConsoleView();
            consoleView.GetMapContent(map, adventurers).Should().Be(mapContent.ToString());
     
            
        }

     
    }
}
