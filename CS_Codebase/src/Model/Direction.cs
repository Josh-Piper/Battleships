using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using static SwinGameSDK.SwinGame;


namespace MyGame
{
    /// <summary>
    /// The direction the ship can be oriented.
    /// </summary>

    public enum Direction
    {
        /// <summary>
        /// The ship is oriented left/right
        /// </summary>
        LeftRight,

        /// <summary>
        /// The ship is oriented up/down
        /// </summary>
        UpDown
    }
}
