using System.Collections.Generic;
using System.Windows.Media;

namespace ModelsEL
{
    /// <summary>
    /// Object made for conatining all the data needed for the presentation layer and the business logic layer to communicate.
    /// A detailed version conatinging more data also exists
    /// </summary>
    public class BlackJackDTO
    {
        public string UserName { get; set; }
        public List<Card> PlayerCards { get; set; }

        public List<Card> DealerCards { get; set; }
        public string DealerCurrentState { get; set; }
        public string PlayerCurrentState { get; set; }
        public string PlayerString { get; set; }
        public string DealerString { get; set; }
        public string CardsLeft { get; set; }
        public int Balance { get; set; }
        public bool StandAvailalle { get; set; }
        public bool HitAvailable { get; set; }
        public bool ShuffleAvailable { get; set; }
        public bool InbetweenRounds { get; set; }
        public float WinPct { get; set; }
    }

    public class BJSetupDTO
    {
        public string UserName { get; set; }
        public int Balance { get; set; }
        public bool StandAvailalle { get; set; }
        public bool HitAvailable { get; set; }
        public bool ShuffleAvailable { get; set; }
        public bool InbetweenRounds { get; set; }
        public string CardsLeft { get; set; }
    }
}