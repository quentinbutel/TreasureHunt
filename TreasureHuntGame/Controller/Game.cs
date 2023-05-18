using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tresor.Model;

namespace TreasureHunt.Controller
{
    public class Game
    {

        private List<Adventurer> adventurers;
        private Map map;
       

        public Game()
        {
            adventurers = new List<Adventurer>();
        }
        public void StartGame()
        {
            try
            {
                // Chemin du fichier à lire
                string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string filesDirectory = Path.Combine(projectDirectory, "filesTest");
                string cheminFichier = Path.Combine(filesDirectory, "test1.txt");

                // Utilisation de StreamReader pour lire le fichier
                using (StreamReader sr = new StreamReader(cheminFichier))
                {
                    string line;
                   
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineContent = line.Split(" - ");
                        switch (lineContent[0])
                        {
                            case "C":
                                map = new Map(int.Parse(lineContent[1]), int.Parse(lineContent[2]));
                                break;
                            case "M":
                                map.AddMountain(int.Parse(lineContent[1]), int.Parse(lineContent[2]));
                                break;
                            case "T":
                                map.AddTreasure(int.Parse(lineContent[1]), int.Parse(lineContent[2]), int.Parse(lineContent[3]));
                                break;
                            case "A":

                                adventurers.Add(new Adventurer(lineContent[1], int.Parse(lineContent[2]), int.Parse(lineContent[3]),  (Orientation)Enum.Parse(typeof(Orientation), lineContent[4]), lineContent[5]));
                               break;
                            default:
                                break;
                        }
                        

                    }
                    GetMap();
                }
            }
            catch (Exception e)
            {
                // Gestion des erreurs
                Console.WriteLine("Une erreur s'est produite : " + e.Message);
            }
        }
        
        public void PlayGame()
        {
            bool stopPlay = false;
           
            while (!stopPlay)
            {
                stopPlay = true;
                foreach (Adventurer adventurer in adventurers)
                {
                    
                        if (adventurer.MovementsSequence.Count > 0)
                        {
                        stopPlay = false;
                            Move(adventurer);
                        }
                        
                    
                }
            }
            foreach (Adventurer adventurer in adventurers)
            {
                Console.WriteLine(adventurer.Name + " " + adventurer.X + " " + adventurer.Y + " " + adventurer.NbTreasureTaken);
            }
            GetMap();




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
            GetMap();
            adventurer.MovementsSequence.RemoveAt(0);
        }
        public bool CanMove(int x, int y)
        {
            bool isAdventurer = adventurers.Any(adventurer => adventurer.X == x && adventurer.Y == y);

            if (map.IsAccessible(x, y) && !isAdventurer)
                return true;

            return false;
        }

        public void GetMap()
        {

            string map = "";
            
            for (int y = 0; y < this.map.Height; y++)
            {
                for (int x = 0; x < this.map.Width; x++)
                {
                    bool isAdventurer = adventurers.Any(adventurer => adventurer.X == x && adventurer.Y == y);
                    if (isAdventurer)
                        Console.Write("A" + adventurers.Find(adventurer => adventurer.X == x && adventurer.Y == y).Name.ToLower().First() + " ");
                    else
                        Console.Write(this.map.MElements[x, y].Symbol + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

        }

        private void AddTreasureToAdventurer(Adventurer adventurer)
        {
            if (map.IsTreasure(adventurer.X, adventurer.Y))
            {
                map.RemoveTreasure(adventurer.X, adventurer.Y);
                adventurer.NbTreasureTaken++;
            }
        }
    }
}
