// See https://aka.ms/new-console-template for more information
using TreasureHunt.Controller;
// Chemin du fichier à lire
string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
string filesDirectory = Path.Combine(projectDirectory, "filesGames");
string fileName = "game1";
string filePath = Path.Combine(filesDirectory, fileName+".txt");
Game game = new Game();
game.StartGame(filePath);
game.PlayGame();
string outputPath = Path.Combine(filesDirectory, fileName+"_EndGame" + ".txt");
game.WriteOutputFile(outputPath);