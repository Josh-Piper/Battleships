
using System.Collections.Generic;


namespace MyGame {

    /// <summary>
    /// A Ship has all the details about itself. For example the shipname,
    /// size, number of hits taken and the location. Its able to add tiles,
    /// remove, hits taken and if its deployed and destroyed.
    /// </summary>
    /// <remarks>
    /// Deployment information is supplied to allow ships to be drawn.
    /// </remarks>
    public class Ship {

        private readonly ShipName _shipName;
        private readonly List<Tile> _tiles;

        /// <summary>
        /// The type of ship
        /// </summary>
        /// <value>The type of ship</value>
        /// <returns>The type of ship</returns>
        public string Name {
            get {
                /////////////////////////////////////////////////////// Todo /////////////////////////////////////////////////////////
                //if (_shipName == ShipName.AircraftCarrier) {
                //    return "Aircraft Carrier";
                //}
                return _shipName.ToString();
            }
        }

        /// <summary>
        /// The number of cells that this ship occupies.
        /// </summary>
        /// <value>The number of hits the ship can take</value>
        /// <returns>The number of hits the ship can take</returns>
        public int Size { get; }

        /// <summary>
        /// The number of hits that the ship has taken.
        /// </summary>
        /// <value>The number of hits the ship has taken.</value>
        /// <returns>The number of hits the ship has taken</returns>
        /// <remarks>When this equals Size the ship is sunk</remarks>
        public int Hits { get; private set; } = 0;

        /// <summary>
        /// The row location of the ship
        /// </summary>
        /// <value>The topmost location of the ship</value>
        /// <returns>the row of the ship</returns>
        public int Row { get; private set; }

        public int Column { get; private set; }

        public Direction Direction { get; private set; }

        /// <summary>
        /// IsDeployed returns if the ships is deployed, if its deplyed it has more than
        /// 0 tiles
        /// </summary>
        public bool IsDeployed {
            get {
                return _tiles.Count > 0;
            }
        }

        public bool IsDestroyed {
            get {
                return Hits == Size;
            }
        }


        public Ship (ShipName ship) {

            _shipName = ship;
            _tiles = new List<Tile>();

            // Get the ship size from the enumarator
            Size = (int)ship;

        }

        /// <summary>
        /// Add tile adds the ship tile
        /// </summary>
        /// <param name="tile">one of the tiles the ship is on</param>
        public void AddTile (Tile tile) {
            _tiles.Add(tile);
        }

        /// <summary>
        /// Remove clears the tile back to a sea tile
        /// </summary>
        public void Remove () {
            foreach (Tile tile in _tiles)
                tile.ClearShip();
            _tiles.Clear();
        }

        public void Hit () {
            Hits = Hits + 1;
        }

        /// <summary>
        /// Record that the ship is now deployed.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        internal void Deployed (Direction direction, int row, int col) {

            Row = row;
            Column = col;
            Direction = direction;

        }

    }

}
