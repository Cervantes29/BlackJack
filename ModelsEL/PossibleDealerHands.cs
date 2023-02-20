using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsEL
{
    /// <summary>
    /// The player hand faces many dealer cards and the dealer cards faces many player hands. A many to many relation and so i created the "Scenario" table in the middle"
    /// This is the only reason this class exists.
    /// </summary>
    public class PossibleDealerHands
    {
        public void SetID(string handID)
        {
            ID = handID;
        }

        [Key]
        public string ID { get; set; }

        public List<Scenario> DealerHand { get; set; }
    }
}