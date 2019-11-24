using System;
using System.Collections.Generic;
using System.Text;

namespace Mazer.Classes
{
    public class Menu
    {
        private Maze _maze = null;
        private Player _player = new Player();

        private enum GameModes
        {
            Unselected,
            Easy,
            Expert,
            Hardcore
        }
        private GameModes _gameMode;

        public Menu()
        {

        }

        /// <summary>
        /// Main menu for running Mazer!
        /// </summary>
        public void MainMenu()
        {
            bool exitNow = false;

            while (!exitNow)
            {
                MainMenuGreeting();

                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1) Play a new Maze.");
                Console.WriteLine("2) Set your name.");
                Console.WriteLine("Q) Exit.");
                Console.WriteLine();

                Console.Write("Enter the number of your selection: ");
                var selection = Console.ReadKey().Key;

                if (selection == ConsoleKey.D1)
                {
                    SelectMazeMenu();
                }
                else if (selection == ConsoleKey.D2)
                {
                    SetNameMenu();
                }
                else if (selection == ConsoleKey.Q)
                {
                    exitNow = true;
                }
            }
        }

        /// <summary>
        /// Greets the player on the initial screen.
        /// </summary>
        private void MainMenuGreeting()
        {
            Console.Clear();
            Console.WriteLine("Welcom to Mazer!");
            Console.WriteLine("Prepare to be a Mazed!");
            Console.WriteLine();
        }

        /// <summary>
        /// Prompts the player to select the maze they want and loads it to the Maze.
        /// </summary>
        private void SelectMazeMenu()
        {
            bool mapSelected = false;
            bool wantstoExit = false;

            while (!mapSelected && !wantstoExit)
            {
                _maze = new Maze();

                Console.Clear();
                Console.WriteLine("It's time to select the maze you would like to run!");
                Console.WriteLine("Your options are:");

                foreach (var maze in _maze.MazeDictionary)
                {
                    // Get the maze location value and rework remove file pathing for displaying.
                    string mazeName = maze.Value;
                    mazeName = mazeName.Substring(0, mazeName.Length - 4);
                    mazeName = mazeName.Substring(15);

                    Console.WriteLine($"{maze.Key}) {mazeName}");
                }

                Console.WriteLine("Q) Return to Main Menu");

                Console.Write("Please enter your selection: ");
                string selection = Console.ReadKey().KeyChar.ToString();
                wantstoExit = selection == "Q" || selection == "q" ||
                            selection == "Quit" || selection == "quit";

                if (!wantstoExit)
                {
                    try
                    {
                        int selectionNumber = int.Parse(selection);
                        _maze.LoadMaze(_maze.MazeDictionary[selectionNumber]);
                        mapSelected = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid selection, please select a valid option.");
                        Console.ReadKey();
                    }
                }

                if (mapSelected)
                {
                    SelectMazeMode();

                    // If player wants to select another maze, reset mapSelected to false.
                    mapSelected = PlayMaze();
                }
            }
        }

        /// <summary>
        /// Prompt the user to select the mode of maze desired.
        /// </summary>
        public void SelectMazeMode()
        {
            while(_gameMode == GameModes.Unselected)
            {
                Console.Clear();
                Console.WriteLine("Great, now to select the mode you would like to play.");
                Console.WriteLine("1) Easy Mode – You can see the whole maze.");
                Console.WriteLine("2) Expert Mode – You can only see your immediate suroundings.");
                Console.WriteLine("3) Hardcore Mode – You are only told if you hit a wall or not.");
                Console.WriteLine();

                Console.Write("What do you choose? ");

                string modeSelected = Console.ReadKey().KeyChar.ToString();
                Enum.TryParse<GameModes>(modeSelected, out _gameMode);
                if (!Enum.IsDefined(typeof(GameModes), _gameMode))
                {
                    _gameMode = GameModes.Unselected;
                }
            }
        }

        /// <summary>
        /// Plays the selected maze and allows the player to select another if they want to.
        /// </summary>
        /// <returns></returns>
        private bool PlayMaze()
        {
            bool noMore = true;
            bool mazeComplete = false;
            bool givesUp = false;
            bool moved = true; // Start ture so it print the first time.

            // Load the player into the map.
            _player.GetMapInfo(_maze.MazeMatrix, _maze.MazeLength, _maze.MazeHeight, _maze.StartPosition);

            while (!mazeComplete && !givesUp)
            {
                if (moved || _gameMode == GameModes.Hardcore)
                {
                    Console.Clear();

                    Console.WriteLine($"Good luck in the {_maze.MazeName}!");

                    if (_gameMode == GameModes.Easy)
                    {
                        _maze.PrintMaze();
                    }
                    else if (_gameMode == GameModes.Expert)
                    {
                        _maze.PrintExpertMaze(_player.PlayerPositionLength, _player.PlayerPositionHeight);
                    }
                    else if (_gameMode == GameModes.Hardcore)
                    {
                        _maze.PrintHardcoreMaze(moved);
                    }

                    Console.WriteLine();
                    Console.WriteLine("      The controls are:");
                    Console.WriteLine("            W = Up");
                    Console.WriteLine("A = Left | S = Down | D = Right");
                    Console.WriteLine("         Q = Give Up");
                }

                var input = Console.ReadKey().Key;
                Console.Write("\b \b"); // Hide user input, to make it pretty.

                if (input == ConsoleKey.W || input == ConsoleKey.UpArrow)
                {
                    moved = _player.MoveUp();
                }
                else if (input == ConsoleKey.S || input == ConsoleKey.DownArrow)
                {
                    moved = _player.MoveDown();
                }
                else if (input == ConsoleKey.D || input == ConsoleKey.RightArrow)
                {
                    moved = _player.MoveRight();
                }
                else if (input == ConsoleKey.A || input == ConsoleKey.LeftArrow)
                {
                    moved = _player.MoveLeft();
                }
                else if (input == ConsoleKey.Q)
                {
                    givesUp = true;
                }

                if (_player.IsAtFinish)
                {
                    mazeComplete = true;
                }
            }

            if (mazeComplete)
            {
                Console.Clear();
                Console.WriteLine($"Awsome Job, {_player.PlayerName}!");
                Console.WriteLine($"You have beaten the {_maze.MazeName}!");
                Console.Write("Would you like to run another? (Y): ");
                var anotherInput = Console.ReadKey().Key;

                noMore = (anotherInput == ConsoleKey.Y) ? false : true;
            }

            // Clear out old maze data so it can't interfere if another is run.
            _maze = null;
            _gameMode = GameModes.Unselected;

            return noMore;
        }

        /// <summary>
        /// Displays the menu for setting player's name.
        /// </summary>
        private void SetNameMenu()
        {
            Console.Clear();
            Console.Write("Enter your name here: ");
            _player.PlayerName = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("        Congratulations!");
            Console.WriteLine("   Your name has been recorded!");
            Console.WriteLine();
            Console.Write("Nothing wrong will be done with it, we Promise!");

            Console.ReadKey();
        }
    }
}
