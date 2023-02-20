/*---------------------------------
 *  Programming in C# III
 *  Assignment 4
 *  By Alfred Geraldsson
 * --------------------------------
 */

using ModelsEL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJackLogicBLL
{
    public class DeckManager
    {
        private const int numberOfEachSuit = 14;
        private const int numberOfSuits = 4;
        private const int minDeckSize = 1;
        private int lastRequest = 1;
        private Random rng = new Random();
        private List<Card> deck;
        internal List<Card> Deck { get => deck; set => deck = value; }

        /// <summary>
        /// Creates a new deck and shuffles it
        /// Takes any number of decks requested
        /// </summary>
        /// <param name="numberOfDecksRequested"></param>
        public void SetUpDecks(int numberOfDecksRequested)
        {
            lastRequest = numberOfDecksRequested;
            Deck = new List<Card>();
            for (int i = 0; i < numberOfDecksRequested; i++)
            {
                for (int suite = 0; suite < numberOfSuits; suite++)
                {
                    for (int value = 1; value < numberOfEachSuit; value++)
                    {
                        Deck.Add(new Card(suite, value));
                    }
                }
            }
            Shuffle(Deck);
        }

        /// <summary>
        /// Draws a card and returns it.
        /// If there are no more cards the deck manager is called to get a new deck
        /// </summary>
        /// <returns></returns>

        public Card DrawCard()
        {
            if (Deck == null || Deck.Count < minDeckSize) SetUpDecks(lastRequest);
            Card cardDrawn = Deck.FirstOrDefault();
            Deck.Remove(cardDrawn);
            return cardDrawn;
        }

        /// <summary>
        /// Shuffles a list of cards using the Fisher - Yates method
        /// </summary>
        /// <typeparam name="Card"></typeparam>
        /// <param name="list"></param>
        public void Shuffle<Card>(List<Card> list)
        {
            int x = list.Count;
            while (x > 1)
            {
                x--;
                int y = rng.Next(x + 1);
                (list[x], list[y]) = (list[y], list[x]);
            }
        }

        /// <summary>
        /// Returns the number of cards from the deck
        /// </summary>
        /// <param name="integer"></param>
        /// <returns></returns>
        public int Count(int integer)
        {
            SetUpDecks(integer);
            return Deck.Count;
        }
    }
}