using System.Data;
using TreasureHunt.Controller;
using TreasureHunt.Model;
using FluentAssertions;
using System.Xml.Linq;


namespace TreasureHunt.UnitTests
{
    [TestClass]
    public class GameTest
    {
        

        [TestMethod]
        public void StartGame_ValidFile_CreatesMapAndAdventurer()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filesDirectory = Path.Combine(projectDirectory, "filesTest");
            string filePath = Path.Combine(filesDirectory, "testNormal.txt");

            Game game = new Game();
            game.StartGame(filePath);

            // Assert
            game.Map.MElements.Should().NotBeNull();
            game.Map.MElements.GetLength(0).Should().Be(5); 
            game.Map.MElements.GetLength(1).Should().Be(5); 

            MotionLessElement element = game.Map.MElements[1, 1]; //mountain coordinates
            element.Should().BeOfType<Mountain>();
            Mountain mountain = (Mountain)element;
            mountain.Should().NotBeNull();
            mountain.Symbol.Should().Be('M');

            element = game.Map.MElements[2, 2]; // treasure coordinates
            element.Should().BeOfType<Treasure>();
            Treasure treasure = (Treasure)element;
            treasure.Should().NotBeNull();
            treasure.Symbol.Should().Be('T');
            treasure.NbTreasure.Should().Be(3);

            
            game.Adventurers.Should().HaveCount(1);
            Adventurer adventurer = game.Adventurers[0];
            adventurer.Name.Should().Be("Indiana");
            adventurer.X.Should().Be(3);
            adventurer.Y.Should().Be(3);
            adventurer.Orientation.Should().Be(Orientation.N); 
            adventurer.MovementsSequence.Should().Equal(
                     Movement.A,
                     Movement.G,
                     Movement.A,
                     Movement.A,
                     Movement.D,
                     Movement.A,
                     Movement.D,
                     Movement.A
                 );
        }

        [TestMethod]
        public void PlayGame_CreatesMapAndAdventurer_FinishWithValidOrientationAndNewPosition()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filesDirectory = Path.Combine(projectDirectory, "filesTest");
            string filePath = Path.Combine(filesDirectory, "testNormal.txt");

            Game game = new Game();
            game.StartGame(filePath);

            game.PlayGame();


            MotionLessElement element = game.Map.MElements[2, 2]; // treasure coordinates
            element.Should().BeOfType<Treasure>();
            Treasure treasure = (Treasure)element;
            treasure.Should().NotBeNull();
            treasure.Symbol.Should().Be('T');
            treasure.NbTreasure.Should().Be(1);
            
            game.Adventurers.Should().HaveCount(1);
            Adventurer adventurer = game.Adventurers[0];
            adventurer.Name.Should().Be("Indiana");
            adventurer.X.Should().Be(2);
            adventurer.Y.Should().Be(2);
            adventurer.NbTreasureTaken.Should().Be(2);
            adventurer.Orientation.Should().Be(Orientation.E);
            adventurer.MovementsSequence.Should().HaveCount(0);

        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void StartGame_InvalidFile_ThrowsFileNotFoundException()
        {
            string filePath = "nonexistent.txt";
            Game game = new Game();
            game.StartGame(filePath);
        }


        [TestMethod]
        public void Move_AdventurerMoves_SameOrientationWithYMinusOne()
        {
            Game game = new Game();
            game.Map = new Map(5, 5);
            game.Map.AddMountain(1,1);
            game.Map.AddTreasure(2,2,2);
            game.Adventurers.Add(new Adventurer("indiana", 3, 3, Orientation.N, "AADADA"));
            // ...
            int expectedX = 3;
            int expectedY = 2;
            Orientation expectedOrientation = Orientation.N;
       
            game.Move(game.Adventurers[0]);

           
            Adventurer adventurer = game.Adventurers[0];
            adventurer.X.Should().Be(expectedX);
            adventurer.Y.Should().Be(expectedY);
            adventurer.Orientation.Should().Be(expectedOrientation);
        }

        [TestMethod]
        public void Move_AdventurerMoves_ReachMountain_DoNotMoveAdventurer()
        {

            Game game = new Game();
            game.Map = new Map(5, 5);
            game.Map.AddMountain(1, 1);
            game.Map.AddTreasure(2, 2, 2);
            game.Adventurers.Add(new Adventurer("indiana", 1, 2, Orientation.N, "AADADA"));
            // ...
            int expectedX = 1;
            int expectedY = 2;
            Orientation expectedOrientation = Orientation.N;

            game.Move(game.Adventurers[0]);


            Adventurer adventurer = game.Adventurers[0];
            adventurer.X.Should().Be(expectedX);
            adventurer.Y.Should().Be(expectedY);
            adventurer.Orientation.Should().Be(expectedOrientation);
        }

        [TestMethod]
        public void Move_AdventurerMoves_PassByTreasure_AddToTreasureTakenAndRemoveFromNbTreasure()
        {

            Game game = new Game();
            game.Map = new Map(5, 5);
            game.Map.AddMountain(1, 1);
            game.Map.AddTreasure(2, 2, 2);
            game.Adventurers.Add(new Adventurer("indiana", 2,3, Orientation.N, "AADADA"));
            // ...
            int expectedX = 2;
            int expectedY = 2;
            Orientation expectedOrientation = Orientation.N;

            game.Move(game.Adventurers[0]);


            Adventurer adventurer = game.Adventurers[0];
            adventurer.X.Should().Be(expectedX);
            adventurer.Y.Should().Be(expectedY);
            adventurer.Orientation.Should().Be(expectedOrientation);
            adventurer.NbTreasureTaken.Should().Be(1);

            MotionLessElement element = game.Map.MElements[2, 2]; // treasure coordinates
            element.Should().BeOfType<Treasure>();
            Treasure treasure = (Treasure)element;
            treasure.NbTreasure.Should().Be(1);

        }
        [TestMethod]
        public void Move_AdventurerMoves_PassByTreasure_AddToTreasureTakenAndRemoveTreasure()
        {

            Game game = new Game();
            game.Map = new Map(5, 5);
            game.Map.AddMountain(1, 1);
            game.Map.AddTreasure(2, 2, 1);
            game.Adventurers.Add(new Adventurer("indiana", 2, 3, Orientation.N, "AADADA"));
            // ...
            int expectedX = 2;
            int expectedY = 2;
            Orientation expectedOrientation = Orientation.N;

            game.Move(game.Adventurers[0]);


            Adventurer adventurer = game.Adventurers[0];
            adventurer.X.Should().Be(expectedX);
            adventurer.Y.Should().Be(expectedY);
            adventurer.Orientation.Should().Be(expectedOrientation);
            adventurer.NbTreasureTaken.Should().Be(1);

            MotionLessElement element = game.Map.MElements[2, 2]; // treasure coordinates
            element.Should().NotBeOfType<Treasure>();
            element.Should().BeOfType<Plain>();

        }

        [TestMethod]
        public void Move_AdventurerMoves_TurnByTurnOrderMatchesAppearanceOrder()
        {

            Game game = new Game();
            game.Map = new Map(5, 5);
            game.Map.AddMountain(1, 1);
            game.Map.AddTreasure(2, 2, 1);
            game.Adventurers.Add(new Adventurer("indiana", 3, 3, Orientation.E, "AADADA"));
            game.Adventurers.Add(new Adventurer("Indiana2", 4, 4, Orientation.N, "AADADA"));
            // ...
            int expectedX = 4;
            int expectedY = 3;
            Orientation expectedOrientation = Orientation.E;

            game.Move(game.Adventurers[0]);


            Adventurer adventurer = game.Adventurers[0];
            adventurer.X.Should().Be(expectedX);
            adventurer.Y.Should().Be(expectedY);
            adventurer.Orientation.Should().Be(expectedOrientation);


             expectedX = 4;
             expectedY = 4;
             expectedOrientation = Orientation.N;
            adventurer = game.Adventurers[1];
            adventurer.X.Should().Be(expectedX);
            adventurer.Y.Should().Be(expectedY);
            adventurer.Orientation.Should().Be(expectedOrientation);


        }
    }
}