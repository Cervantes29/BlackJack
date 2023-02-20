using System.ComponentModel.DataAnnotations;

namespace ModelsEL
{
    /// <summary>
    /// For logging every relevant information after every hand
    /// </summary>
    public class Gamelog
    {
        public Gamelog(
            int userID,
            string playerCards,
            string dealerCards,
            int playerScore,
            int dealerScore,
            float balanceChange,
            bool decision,
            string outcome
            ) : this()
        {
            UserID = userID;
            PlayerCards = playerCards;
            DealerCards = dealerCards;
            PlayerScore = playerScore;
            DealerScore = dealerScore;
            BalanceChange = balanceChange;
            Decision = decision;
            Outcome = outcome;
        }

        public Gamelog()
        {
            UserID = 0;
            PlayerCards = "test QQ";
            DealerCards = "Test AA";
            PlayerScore = 0;
            DealerScore = 0;
            BalanceChange = 0;
            Outcome = "Push";
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string PlayerCards { get; set; }

        [Required]
        public string DealerCards { get; set; }

        [Required]
        public int PlayerScore { get; set; }

        [Required]
        public int DealerScore { get; set; }

        [Required]
        public float BalanceChange { get; set; }

        [Required]
        public bool Decision { get; set; }

        public string Outcome { get; set; }
    }
}