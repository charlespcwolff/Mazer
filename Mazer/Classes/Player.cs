using System;
using System.Collections.Generic;
using System.Text;

namespace Mazer.Classes
{
    public class Player
    {
        private const string FLOOR = " ";

        public string PlayerName { get; set; } = "Player";
        public string PlayerSymbol { get; set; } = "☺";
        public int PlayerPositionHeight { get; private set; }
        public int PlayerPositionLength { get; private set; }
        public bool IsAtFinish
        {
            get { return Map[PlayerPositionHeight][PlayerPositionLength].IsGoal; }
        }

        // To store the map, so player can see if can move.
        private List<List<Tile>> Map { get; set; }
        private int MapLength { get; set; }
        private int MapHeight { get; set; }

        public Player()
        {

        }

        /// <summary>
        /// Retrieves map from the Maze so player can see where to start and if it can move.
        /// </summary>
        /// <param name="map"></param>
        public void GetMapInfo(List<List<Tile>> map, int mapLength, int mapHeight, int[] startPosition)
        {
            Map = map;
            MapHeight = mapHeight;
            MapLength = mapLength;
            PlayerPositionHeight = startPosition[0];
            PlayerPositionLength = startPosition[1];
            Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = PlayerSymbol;
        }

        /// <summary>
        /// Checks if the player can move up and if so, does so. Returns true if moved.
        /// </summary>
        public bool MoveUp()
        {
            bool didMove = false;

            if (Map[PlayerPositionHeight - 1][PlayerPositionLength].AllowsMovement)
            {
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = FLOOR;
                PlayerPositionHeight--;
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = PlayerSymbol;

                didMove = true;
            }

            return didMove;
        }

        /// <summary>
        /// Checks if the player can move down, and if so does so. Returns true if moved.
        /// </summary>
        public bool MoveDown()
        {
            bool didMove = false;

            if (Map[PlayerPositionHeight + 1][PlayerPositionLength].AllowsMovement)
            {
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = FLOOR;
                PlayerPositionHeight++;
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = PlayerSymbol;

                didMove = true;
            }

            return didMove;
        }

        /// <summary>
        /// Checks if the player can move left, and if so does so. Returns true if moved.
        /// </summary>
        public bool MoveLeft()
        {
            bool didMove = false;

            if (Map[PlayerPositionHeight][PlayerPositionLength - 1].AllowsMovement)
            {
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = FLOOR;
                PlayerPositionLength--;
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = PlayerSymbol;

                didMove = true;
            }

            return didMove;
        }

        /// <summary>
        /// Checks if the player can move right, and if so does so. Returns true if moved.
        /// </summary>
        public bool MoveRight()
        {
            bool didMove = false;

            if (Map[PlayerPositionHeight][PlayerPositionLength + 1].AllowsMovement)
            {
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = FLOOR;
                PlayerPositionLength++;
                Map[PlayerPositionHeight][PlayerPositionLength].TileSymbol = PlayerSymbol;

                didMove = true;
            }

            return didMove;
        }
    }
}
