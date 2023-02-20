using ModelsEL;
using System.Collections.Generic;

namespace BlackJackLogicBLL

{
    /// <summary>
    /// Contains all logic for a hand
    /// Each player has a hand, and every hand has zero or more cards.
    /// </summary>
    public class Hand
    {
        public Hand()
        {
            Cards = new List<Card>();
        }

        private List<Card> cards;
        public List<Card> Cards { get => cards; set => cards = value; }
        public int Count => Cards.Count;

        /// <summary>
        /// Calculates the total hand value
        /// </summary>
        /// <returns></returns>
        public int HandValueTotal()
        {
            int score = 0;
            int aces = 0;
            if (Cards != null)
            {
                foreach (Card card in Cards)
                {
                    if (card.Value >= 10 && card.Value != 1) score += 10;
                    else score += card.Value;
                    if (card.Value == 1) aces++;
                }

                for (int i = 0; i < aces; i++)
                {
                    if (score <= 11) score += 10;
                }
            }
            return score;
        }

        /// <summary>
        /// Clears the hand
        /// </summary>
        public void Clear() => Cards?.Clear();

        /// <summary>
        /// Returns an array of strings that contains the resource paths to the cards on hand
        /// </summary>
        /// <returns></returns>
        public List<string> CardImageSources()
        {
            List<string> sources = new();
            foreach (Card card in Cards) sources.Add(card.FileName);
            return sources;
        }

        /// <summary>
        /// Adds a cards to the hand
        /// </summary>
        /// <param name="card"></param>
        public void Add(Card card) => Cards.Add(card);

        /// <summary>
        /// Removes a card
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Card item) => Cards.Remove(item);

        /// <summary>
        /// Removes a card at a spcific index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) => Cards.RemoveAt(index);

        /// <summary>
        /// Returns a represntation of the hand as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string hand = "";
            foreach (Card card in Cards)
            {
                hand += card.Value.ToString();
                hand += card.Suite.ToString();
                hand += ", ";
            }
            return hand;
        }

        /// <summary>
        /// Returns an abbreviated represntation of the hand as a string designed to only contain neccesary information about the value of the hand
        /// </summary>
        /// <returns></returns>
        public string ChartAbbreviationToString()
        {
            string hand = "";
            foreach (Card card in Cards)
            {
                hand += card.Value.ToString();
                hand += ",";
            }
            return hand;
        }
    }
}