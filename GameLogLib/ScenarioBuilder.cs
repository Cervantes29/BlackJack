using DataAccessDAL;
using Microsoft.EntityFrameworkCore;
using ModelsEL;
using System.Collections.Generic;
using System.Linq;

//using UtilitiesLib;

namespace GamelogLib
{
    /// <summary>
    /// A class that sets up three databases by extension. The idea is a chart for each dealer cards versus every player hand that togheter creates a unique scenario
    /// Not fully funvtional yet.
    /// </summary>
    public class ScenarioBuilder
    {
        private List<Scenario> scenarios;
        private PossibleHandsDBGenerator possibleHandsDBGenerator = new();

        public ScenarioBuilder()
        {
            SaveScenariosToDatabase(CreateChart());
        }

        public List<Scenario> Scenarios { get => scenarios; set => scenarios = value; }

        /// <summary>
        /// Creates a chatrt with all possible combination of hands
        /// Not optimized. Contains dubblettes
        /// </summary>
        /// <returns></returns>
        public List<Scenario> CreateChart()
        {
            List<Scenario> scenarios = new();
            List<string> dealerHands = possibleHandsDBGenerator.DealerPossibleHandsList();
            List<string> playerHands = possibleHandsDBGenerator.PlayerPossibleHandsList();

            foreach (string cardsD in dealerHands)
            {
                foreach (string cardsP in playerHands)
                {
                    Scenario scenario = new(1, cardsP, cardsD);

                    scenarios.Add(scenario);
                }
            }
            return scenarios;
        }

        /// <summary>
        /// Adds a scenario to database
        /// </summary>
        /// <param name="Scenario"></param>
        public void Add(Scenario Scenario)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new(optionsBuilder.Options);
            context.Update(Scenario);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets all scenarios in database
        /// </summary>
        /// <returns></returns>
        public List<Scenario> GetAllScenarios()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using var db = new GameDbContext(optionsBuilder.Options);
            var Scenarios = (from dbScenario in db.Scenarios
                             select new Scenario(
                    dbScenario.Round,
                    dbScenario.PlayerHand
                             , dbScenario.DealerHand
                             )).ToList();
            return Scenarios;
        }

        /// <summary>
        /// Saves all scenarios to database
        /// </summary>
        /// <param name="Scenarios"></param>
        private void SaveScenariosToDatabase(List<Scenario> Scenarios)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new(optionsBuilder.Options);
            int count = Scenarios.Count;

            for (int i = 0; i < count; i++)
            {
                Scenario Scenario = Scenarios[i];
                context.Scenarios.Add(Scenario);
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Clears all data in database
        /// </summary>
        private void Clear()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            int count = GetAllScenarios().Count;

            foreach (var entity in context.Scenarios)
                context.Scenarios.Remove(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Removes an scenario entry in the database
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new(optionsBuilder.Options);
            context.Remove(GetAllScenarios()[index]);
            context.SaveChanges();
        }
    }
}