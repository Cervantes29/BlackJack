using System.ComponentModel.DataAnnotations;

namespace ModelsEL
{
    public class Scenario
    {
        /// <summary>
        /// Initial idea was to create a relation table to cover blakcjack strategies.
        /// The player hand faces many different dealer cards and many different scenarios can happen. Each with a mathmaticaly more correct decision.
        /// A relation might not have been the best way to set this up.
        /// </summary>
        /// <param name="round"></param>
        /// <param name="playerHand"></param>
        /// <param name="dealerHand"></param>
        public Scenario(int round, string playerHand, string dealerHand)
        {
            Round = round;
            PlayerHand = playerHand;
            DealerHand = dealerHand;
            //PossiblePlayerHands = playerHand;
            //PossibleDealerHands = dealerHand;
            Decision = "Stand";
        }

        [Key]
        public int ScenarioID { get; set; }

        public int Round { get; set; }

        //public string PossibleDealerHands { get; set; }
        public string PlayerHand { get; set; }

        public string DealerHand { get; set; }

        //[ForeignKey("PossibleDealerHands")]
        //[ForeignKey(nameof(DealerHand))]
        //[Required]
        //public List<PossibleDealerHands> DealerHands { get; set; }
        //[ForeignKey("PossibleDealerHand")]
        //public string HandID { get; set; }

        public string Decision { get; set; }
    }
}