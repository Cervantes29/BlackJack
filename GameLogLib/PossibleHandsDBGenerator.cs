using DataAccessDAL;
using Microsoft.EntityFrameworkCore;
using ModelsEL;
using System.Collections.Generic;

namespace GamelogLib
{
    public class PossibleHandsDBGenerator
    {
        /// <summary>
        /// Creates a chart (relation table) of all the possible starting hand scenarios.
        /// </summary>
        public PossibleHandsDBGenerator()
        {
            Clear();
            List<string> dealerHands = DealerPossibleHandsList();
            List<string> playerHands = PlayerPossibleHandsList();
            SavePossibleHandsToDatabase(dealerHands, playerHands);
        }

        /// <summary>
        /// Saves the possible hands combinations to database where a foreign key to the Scenario table makes up a chart of all the possible hand starting combinations (or it will soon anyway).
        /// </summary>
        /// <param name="dealerHands"></param>
        /// <param name="playerHands"></param>
        private void SavePossibleHandsToDatabase(List<string> dealerHands, List<string> playerHands)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            int dealerCount = dealerHands.Count;
            int playerCount = playerHands.Count;
            for (int i = 0; i < dealerCount; i++)
            {
                PossibleDealerHands hand = new();
                hand.SetID(dealerHands[i]);
                context.PossibleDealerHands.Add(hand);
            }
            for (int i = 0; i < playerCount; i++)
            {
                PossiblePlayerHands hand = new();
                hand.SetID(playerHands[i]);
                context.PossiblePlayerHands.Add(hand);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Clears the database from all entries
        /// </summary>
        private void Clear()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);

            foreach (var entity in context.PossiblePlayerHands)
                context.PossiblePlayerHands.Remove(entity);

            foreach (var entity in context.PossibleDealerHands)
                context.PossibleDealerHands.Remove(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Creates alist of the possible cards teh dealer can have
        /// </summary>
        /// <returns></returns>
        public List<string> DealerPossibleHandsList()
        {
            List<string> strings = new();

            strings.Add("A");
            for (int i = 2; i < 11; i++)
            {
                strings.Add(i.ToString());
            }
            return strings;
        }

        /// <summary>
        /// Creates a list of strings with all possible player starting hand abbreviations
        /// </summary>
        /// <returns></returns>
        public List<string> PlayerPossibleHandsList()
        {
            List<string> initialHand = new();

            initialHand.Add("A,A");

            for (int i = 2; i < 11; i++)
            {
                string ace = "A";
                string card = i.ToString();
                initialHand.Add(ace + "," + card);
            }

            int checkednr = 2;
            for (int i = 2; i < 11; i++)
            {
                for (int x = checkednr; x < 11; x++)
                {
                    string firstCard = i.ToString();
                    string secondCard = x.ToString();
                    initialHand.Add(firstCard + "," + secondCard);
                }
                checkednr++;
            }

            return initialHand;
        }
    }
}