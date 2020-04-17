
using System;


namespace Battleships {

    /// <summary>
    /// Tile knows its location on the grid, if it is a ship and if it has been
    /// shot before
    /// </summary>
    public class Tile {

        private Ship _Ship = default;     // the ship the tile belongs to

        /// <summary>
        /// Has the tile been shot?
        /// </summary>
        /// <value>indicate if the tile has been shot</value>
        /// <returns>true if the tile was shot</returns>
        public bool Shot { get; set; } = false;

        /// <summary>
        /// The row of the tile in the grid
        /// </summary>
        /// <value>the row index of the tile in the grid</value>
        /// <returns>the row index of the tile</returns>
        public int Row { get; }

        /// <summary>
        /// The column of the tile in the grid
        /// </summary>
        /// <value>the column of the tile in the grid</value>
        /// <returns>the column of the tile in the grid</returns>
        public int Column { get; }

        /// <summary>
        /// Ship allows for a tile to check if there is ship and add a ship to a tile
        /// </summary>
        public Ship Ship {
            get {
                return _Ship;
            }
            set {
                if (_Ship is null) {
                    _Ship = value;
                    if (value is object) {
                        _Ship.AddTile(this);
                    }
                }
                else {
                    throw new InvalidOperationException("There is already a ship at [" + Row + ", " + Column + "]");
                }
            }
        }

        /// <summary>
        /// View is able to tell the grid what the tile is
        /// </summary>
        public TileView View {
            get {
                // if there is no ship in the tile
                if (_Ship is null) {
                    // and the tile has been hit
                    if (Shot) {
                        return TileView.Miss;
                    }
                    else {
                        // and the tile hasn't been hit
                        return TileView.Sea;
                    }
                }
                // if there is a ship and it has been hit
                else if (Shot) {
                    return TileView.Hit;
                }
                else {
                    // if there is a ship and it hasn't been hit
                    return TileView.Ship;
                }
            }
        }


        /// <summary>
        /// The tile constructor will know where it is on the grid, and is its a ship
        /// </summary>
        /// <param name="row">the row on the grid</param>
        /// <param name="col">the col on the grid</param>
        /// <param name="ship">what ship it is</param>
        public Tile (int row, int col, Ship ship) {

            Row = row;
            Column = col;
            _Ship = ship;

        }

        /// <summary>
        /// Clearship will remove the ship from the tile
        /// </summary>
        public void ClearShip () {

            _Ship = null;

        }

        /// <summary>
        /// Shoot allows a tile to be shot at, and if the tile has been hit before
        /// it will give an error
        /// </summary>
        internal void Shoot () {

            if (Shot == false) {
                Shot = true;
                if (_Ship is object) {
                    _Ship.Hit();
                }
            }
            else {
                throw new ApplicationException("You have already shot this square");
            }

        }

    }

}
