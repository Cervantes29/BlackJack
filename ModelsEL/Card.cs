namespace ModelsEL

{
    /// <summary>
    /// Card representation
    /// </summary>
    public class Card
    {
        private string fileName = "jb.png";
        private SuiteShorthand suite;
        private int value = 0;
        public string FileName { get => fileName; set => fileName = value; }
        public SuiteShorthand Suite { get => suite; set => suite = value; }
        public int Value { get => value; set => this.value = value; }

        public Card() : this(SuiteShorthand.h, 1)
        {
        }

        /// <summary>
        /// Converts an enum (suite) and an int (value) to a string that can be interpreted as a string representation of a playing card
        /// </summary>
        /// <param name="suite"></param>
        /// <param name="value"></param>
        public Card(SuiteShorthand suite, int value)
        {
            Value = value;
            Suite = suite;
            FileName = $"/resources/{Suite}{value}.png";
        }

        /// <summary>
        /// Converts an int (suiteInt) and an int (value) to a string that can be interpreted as a string representation of a playing card
        /// </summary>
        /// <param name="suiteInt"></param>
        /// <param name="value"></param>
        public Card(int suiteInt, int value)
        {
            Value = value;
            Suite = (SuiteShorthand)suiteInt;
            FileName = $"/resources/{Suite}{value}.png";
        }
    }
}