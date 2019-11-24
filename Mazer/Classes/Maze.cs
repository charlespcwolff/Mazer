using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mazer.Classes
{
    public class Maze
    {
        public const char WALL = '#';
        public const char HORIZONTAL_WALL = '═';
        public const char VERTICAL_WALL = '║';
        public const char TOP_RIGHT_WALL = '╗';
        public const char TOP_LEFT_WALL = '╔';
        public const char BOTTOM_RIGHT_WALL = '╝';
        public const char BOTTOM_LEFT_WALL = '╚';
        public const char INTERSECTION_TOP_WALL = '╦';
        public const char INTERSECTION_BOTTOM_WALL = '╩';
        public const char INTERSECTION_RIGHT_WALL = '╣';
        public const char INTERSECTION_LEFT_WALL = '╠';
        public const char INTERSECTION_FOURWAY = '╬';
        public const char FLOOR = ' ';
        public const char START_POSITION = '☺';
        public const char EXIT = 'X';
        public const string MAZE_FILES = @"..\..\..\Mazes";

        public string MazeName { get; private set; }
        public int MazeLength { get; private set; }
        public int MazeHeight { get; private set; }
        public List<List<Tile>> MazeMatrix { get; private set; } = new List<List<Tile>>();
        public int[] StartPosition { get; private set; }
        public Dictionary<int, string> MazeDictionary
        {
            get
            {
                var mazeDictionary = new Dictionary<int, string>();
                string[] mazeFiles = Directory.GetFiles(MAZE_FILES);
                int mazeCount = 1;

                foreach(var maze in mazeFiles)
                {
                    mazeDictionary.Add(mazeCount, maze);
                    mazeCount++;
                }

                return mazeDictionary;
            }
        }

        public Maze()
        {

        }

        /// <summary>
        /// Prints a copy of the map to the screen.
        /// </summary>
        public void PrintMaze()
        {
            foreach (List<Tile> mazeLine in MazeMatrix)
            {
                foreach(Tile tile in mazeLine)
                {
                    Console.Write(tile.TileSymbol);
                }

                Console.WriteLine(); // Write new line, for end of row.
            }
        }

        /// <summary>
        /// Prints out the maze, showing the player only the tiles
        /// immediately surrounding them.
        /// </summary>
        /// <param name="playerX"></param>
        /// <param name="playerY"></param>
        public void PrintExpertMaze(int playerX, int playerY)
        {
            Console.Write(MazeMatrix[playerY - 1][playerX - 1].TileSymbol.PadLeft(14));
            Console.Write(MazeMatrix[playerY - 1][playerX].TileSymbol);
            Console.WriteLine(MazeMatrix[playerY - 1][playerX + 1].TileSymbol);

            Console.Write(MazeMatrix[playerY][playerX - 1].TileSymbol.PadLeft(14));
            Console.Write(MazeMatrix[playerY][playerX].TileSymbol);
            Console.WriteLine(MazeMatrix[playerY][playerX + 1].TileSymbol);

            Console.Write(MazeMatrix[playerY + 1][playerX - 1].TileSymbol.PadLeft(14));
            Console.Write(MazeMatrix[playerY + 1][playerX].TileSymbol);
            Console.WriteLine(MazeMatrix[playerY + 1][playerX + 1].TileSymbol);
        }

        /// <summary>
        /// Prints out only messages of whether player was able to sucessfully
        /// move or not.
        /// </summary>
        /// <param name="moved"></param>
        public void PrintHardcoreMaze(bool moved)
        {
            Console.WriteLine();

            if (moved)
            {
                Console.WriteLine("You have moved forward.");
            }
            else
            {
                Console.WriteLine("You run headfirst into a wall. Ouch!");
            }
        }

        /// <summary>
        /// Loads the input file, to create the maze.
        /// </summary>
        public void LoadMaze(string mazeFile)
        {
            try
            {
                using (StreamReader sr = new StreamReader(mazeFile))
                {
                    // Leave the first 4 lines off adding to map, map info on them.
                    int mapHeightIndex = -4;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (mapHeightIndex >= 0)
                        {
                            char[] lineChars = line.ToCharArray();

                            MazeMatrix.Add(new List<Tile>());

                            for (int i = 0; i < lineChars.Length; i++)
                            {
                                char character = lineChars[i];

                                if (character == WALL || character == HORIZONTAL_WALL
                                    || character == VERTICAL_WALL || character == TOP_LEFT_WALL
                                    || character == TOP_RIGHT_WALL || character == BOTTOM_LEFT_WALL
                                    || character == BOTTOM_RIGHT_WALL || character == INTERSECTION_TOP_WALL
                                    || character == INTERSECTION_LEFT_WALL || character == INTERSECTION_RIGHT_WALL
                                    || character == INTERSECTION_BOTTOM_WALL || character == INTERSECTION_FOURWAY)
                                {
                                    MazeMatrix[mapHeightIndex].Add(new Tile(character.ToString(), false));
                                }
                                else if (character == START_POSITION)
                                {
                                    StartPosition = new int[] { mapHeightIndex, i };
                                    MazeMatrix[mapHeightIndex].Add(new Tile());
                                }
                                else if (character == EXIT)
                                {
                                    MazeMatrix[mapHeightIndex].Add(
                                        new Tile(character.ToString(), true, true));
                                }
                                else
                                {
                                    MazeMatrix[mapHeightIndex].Add(new Tile());
                                }
                            }
                        }
                        else
                        {
                            if(mapHeightIndex == -4)
                            {
                                MazeName = line.Substring(11);
                            }
                            else if (mapHeightIndex == -3)
                            {
                                MazeLength = int.Parse(line.Substring(12));
                            }
                            else if (mapHeightIndex == -2)
                            {
                                MazeHeight = int.Parse(line.Substring(12));
                            }
                        }

                        mapHeightIndex++;
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Error reading the maze.");
            }
        }
    }
}
