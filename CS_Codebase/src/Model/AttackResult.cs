
namespace MyGame {

    /// <summary>
    /// AttackResult gives the result after a shot has been made.
    /// </summary>
    public class AttackResult {

        /// <summary>
        /// The result of the attack
        /// </summary>
        /// <value>The result of the attack</value>
        /// <returns>The result of the attack</returns>
        public ResultOfAttack Value { get; }

        /// <summary>
        /// The ship, if any, involved in this result
        /// </summary>
        /// <value>The ship, if any, involved in this result</value>
        /// <returns>The ship, if any, involved in this result</returns>
        public Ship Ship { get; }

        /// <summary>
        /// A textual description of the result.
        /// </summary>
        /// <value>A textual description of the result.</value>
        /// <returns>A textual description of the result.</returns>
        /// <remarks>A textual description of the result.</remarks>
        public string Text { get; }

        /// <summary>
        /// The row where the attack occurred
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// The column where the attack occurred
        /// </summary>
        public int Column { get; }


        /// <summary>
        /// Set the _Value to the PossibleAttack value
        /// </summary>
        /// <param name="value">either hit, miss, destroyed, shotalready</param>
        public AttackResult (ResultOfAttack value, string text, int row, int column) {

            Value = value;
            Text = text;
            Row = row;
            Column = column;

        }

        /// <summary>
        /// Set the _Value to the PossibleAttack value, and the _Ship to the ship
        /// </summary>
        /// <param name="value">either hit, miss, destroyed, shotalready</param>
        /// <param name="ship">the ship information</param>
        public AttackResult (ResultOfAttack value, Ship ship, string text, int row, int column)
            : this(value, text, row, column) {

            Ship = ship;

        }

        /// <summary>
        /// Displays the textual information about the attack
        /// </summary>
        /// <returns>The textual information about the attack</returns>
        public override string ToString () {

            if (Ship is null) {
                return Text;
            }

            return Text + " " + Ship.Name;

        }

    }

}
