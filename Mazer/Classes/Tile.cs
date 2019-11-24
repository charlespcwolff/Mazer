using System;
using System.Collections.Generic;
using System.Text;

namespace Mazer.Classes
{
    public class Tile
    {
        private const string FLOOR = " ";

        public string TileSymbol { get; set; }
        public bool AllowsMovement { get; }
        public bool IsGoal { get; }

        /// <summary>
        /// When creating new tile: set the symbol the tile will display and if it allows movement,
        /// default is an floor tile and allows movement, not the goal of the maze.
        /// </summary>
        /// <param name="tileSymbol"></param>
        /// <param name="allowsMovement"></param>
        public Tile(string tileSymbol = FLOOR, bool allowsMovement = true, bool isGoal = false)
        {
            TileSymbol = tileSymbol;
            AllowsMovement = allowsMovement;
            IsGoal = isGoal;
        }
    }
}
