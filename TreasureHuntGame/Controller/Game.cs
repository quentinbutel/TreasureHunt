using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TreasureHunt.View;
using TreasureHunt.Model;

namespace TreasureHunt.Controller
{
    public class Game
    {
        public List<Adventurer> Adventurers { get; set; }
        public Map Map { get; set; }
        
        public ConsoleView ConsoleView { get; set; }
        public Game()
        {
            ConsoleView = new();
            Adventurers = new List<Adventurer>();
        }

        
        public void StartGame(string filePath)
        {
             

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                   
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineContent = line.Split(" - ");
                        switch (lineContent[0])
                        {
                            case "C":
                                if (lineContent.Length == 3)
                                    Map = new Map(int.Parse(lineContent[1]), int.Parse(lineContent[2]));
                                break;
                            case "M":
                                if (Map != null && lineContent.Length == 3)
                                    Map.AddMountain(int.Parse(lineContent[1]), int.Parse(lineContent[2]));
                                break;
                            case "T":
                                if (Map != null && lineContent.Length == 4)
                                    Map.AddTreasure(int.Parse(lineContent[1]), int.Parse(lineContent[2]), int.Parse(lineContent[3]));
                                break;
                            case "A":
                                if ( lineContent.Length == 6)
                                Adventurers.Add(new Adventurer(lineContent[1], int.Parse(lineContent[2]), int.Parse(lineContent[3]),  (Orientation)Enum.Parse(typeof(Orientation), lineContent[4]), lineContent[5]));
                               break;
                            default:
                                break;
                        }
                        

                    }
                   Console.WriteLine( ConsoleView.GetMapContent(Map, Adventurers));
                }
            }
            
        
        public void PlayGame()
        {
            bool stopPlay = false;
            if (Map == null)
                stopPlay = true;
            while (!stopPlay)
            {
                stopPlay = true;
                foreach (Adventurer adventurer in Adventurers)
                {
                    
                        if (adventurer.MovementsSequence.Count > 0)
                        {
                        stopPlay = false;
                            Move(adventurer);
                        }
                        
                    
                }
            }

            Console.WriteLine(ConsoleView.GetMapContent(Map, Adventurers));



        }
        public void Move(Adventurer adventurer)
        {
            switch (adventurer.MovementsSequence.First())
            {
                case Movement.A:
                    switch (adventurer.Orientation)
                    {
                        case Orientation.N:
                            if (CanMove(adventurer.X, adventurer.Y - 1))
                            {
                                adventurer.Y -= 1;
                                AddTreasureToAdventurer(adventurer);
                            }
                                
                            break;
                        case Orientation.S:
                            if (CanMove(adventurer.X, adventurer.Y + 1))
                            {
                                adventurer.Y += 1;
                                AddTreasureToAdventurer(adventurer);
                            }
                            break;
                        case Orientation.E:
                            if (CanMove(adventurer.X+1, adventurer.Y))
                            {
                                adventurer.X += 1;
                                AddTreasureToAdventurer(adventurer);
                            }
                            break;
                        case Orientation.W:
                            if (CanMove(adventurer.X - 1, adventurer.Y))
                            {
                                adventurer.X -= 1;
                                AddTreasureToAdventurer(adventurer);
                            }
                            break;
                    }
                    
                    break;
                case Movement.D:
                    switch (adventurer.Orientation)
                    {
                        case Orientation.N:
                            adventurer.Orientation = Orientation.E;
                            break;
                        case Orientation.S:
                            adventurer.Orientation = Orientation.W;
                            break;
                        case Orientation.E:
                            adventurer.Orientation = Orientation.S;
                            break;
                        case Orientation.W:
                            adventurer.Orientation = Orientation.N;
                            break;
                    }
                    break;
                case Movement.G:
                    switch (adventurer.Orientation)
                    {
                        case Orientation.N:
                            adventurer.Orientation = Orientation.W;
                            break;
                        case Orientation.S:
                            adventurer.Orientation = Orientation.E;
                            break;
                        case Orientation.E:
                            adventurer.Orientation = Orientation.N;
                            break;
                        case Orientation.W:
                            adventurer.Orientation = Orientation.S;
                            break;
                    }
                    break;
                default:
                    break;
            }
            adventurer.MovementsSequence.RemoveAt(0);
        }
        public bool CanMove(int x, int y)
        {
            bool isAdventurer = Adventurers.Any(adventurer => adventurer.X == x && adventurer.Y == y);

            if (Map.IsAccessible(x, y) && !isAdventurer)
                return true;

            return false;
        }

       

        private void AddTreasureToAdventurer(Adventurer adventurer)
        {
            if (Map.IsTreasure(adventurer.X, adventurer.Y))
            {
                Map.RemoveTreasure(adventurer.X, adventurer.Y);
                adventurer.NbTreasureTaken++;
            }
        }

        public void WriteOutputFile(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine($"C - {Map.Width} - {Map.Height}");
                for (int y = 0; y < Map.Height; y++)
                {
                    for (int x = 0; x < Map.Width; x++)
                    {
                        switch (Map.MElements[x, y])
                        {
                            case Mountain mountain:
                                sw.WriteLine($"M - {x} - {y}");

                                break;
                            case Treasure treasure:
                                sw.WriteLine($"T - {x} - {y} - {treasure.NbTreasure}");
                                break;
                            default:
                                break;
                        }
                    }
                    Console.WriteLine();
                }
                foreach (Adventurer adventurer in Adventurers)
                {
                    // Écrire les informations de l'aventurier dans le fichier
                    sw.WriteLine($"A - {adventurer.Name} - {adventurer.X} - {adventurer.Y} - {adventurer.Orientation} - {adventurer.NbTreasureTaken}");
                }
            }
        }
    }
}
