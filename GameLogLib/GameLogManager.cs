//using BlackJackStrategyLib;
using DataAccessDAL;
using Elsa.Models;
using Microsoft.EntityFrameworkCore;
using ModelsEL;
using Rebus.Pipeline.Send;
using System.Collections.Generic;
using System.Linq;

namespace GamelogLib
{
    public class GamelogManager
    {
        private List<Gamelog> gamelogs;
        private List<Gamelog> Gamelogs { get => gamelogs; set => gamelogs = value; }
        private PossibleHandsDBGenerator possibleHandsDBGenerator = new();
        private ScenarioBuilder scenarioBuilder = new();

        public GamelogManager()
        {
            GetAllGamelogs();
        }

        /// <summary>
        /// Gets the gamelogs of a specified user
        /// </summary>
        /// <param name="searchID"></param>
        /// <returns></returns>
        public Gamelog GetGamelog(int searchID)
        {
            Gamelog userGamelogs = new Gamelog();
            for (int i = 0; i < GetAllGamelogs().Count; i++)
            {
                if (!GetAllGamelogs()[i].Id.Equals(searchID)) ;
                {
                    userGamelogs = GetAllGamelogs()[i];
                }
            }
            return userGamelogs;
        }

        /// <summary>
        /// Gets all the gamelogs for a provided user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Gamelog> GetGamelogs(User user)
        {
            List<Gamelog> gamelogs = new();
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using var db = new GameDbContext(optionsBuilder.Options);
            {
                gamelogs = (from Gamelog in db.Gamelogs
                            where Gamelog.UserID == user.Id
                            select new Gamelog(
                    Gamelog.UserID,
                    Gamelog.PlayerCards,
                    Gamelog.DealerCards,
                    Gamelog.PlayerScore,
                    Gamelog.DealerScore,
                    Gamelog.BalanceChange,
                    Gamelog.Decision,
                    Gamelog.Outcome
                             )).ToList();
            }
            return gamelogs;
        }

        /// <summary>
        /// Removes an gamelog entry in the database
        /// </summary>
        /// <param name="index"></param>
        public void RemoveUserLogs(int userId)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            var userGamelogs = context.Gamelogs.Where(gl => gl.UserID == userId);
            context.Gamelogs.RemoveRange(userGamelogs);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets all gamelogs in database
        /// </summary>
        /// <returns></returns>
        public List<Gamelog> GetAllGamelogs()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using var db = new GameDbContext(optionsBuilder.Options);
            var Gamelogs = (from dbGamelog in db.Gamelogs
                            select new Gamelog(
                   dbGamelog.UserID,
                   dbGamelog.PlayerCards,
                   dbGamelog.DealerCards,
                   dbGamelog.PlayerScore,
                   dbGamelog.DealerScore,
                   dbGamelog.BalanceChange,
                   dbGamelog.Decision,
                   dbGamelog.Outcome
                            )).ToList();
            return Gamelogs;
        }

        /// <summary>
        /// Saves all gamelogs to database
        /// </summary>
        /// <param name="Gamelogs"></param>
        private void SaveGamelogsToDatabase(List<Gamelog> Gamelogs)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            int count = GetAllGamelogs().Count;

            for (int i = 0; i < count; i++)
            {
                Gamelog Gamelog = Gamelogs[i];
                context.Gamelogs.Add(Gamelog);
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
            int count = GetAllGamelogs().Count;

            foreach (var entity in context.Gamelogs)
                context.Gamelogs.Remove(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Replaces en gamelog entry in the databse
        /// </summary>
        /// <param name="Gamelog"></param>
        public void Insert(Gamelog Gamelog)
        {
            GetGamelog(Gamelog.Id);
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            {
                Gamelog result = context.Gamelogs.SingleOrDefault(IdCompare => IdCompare.Id == Gamelog.Id);
                if (result != null)
                {
                    result.Id = Gamelog.Id;
                    result.UserID = Gamelog.UserID;
                    result.PlayerCards = Gamelog.PlayerCards;
                    result.DealerCards = Gamelog.DealerCards;
                    result.PlayerScore = Gamelog.PlayerScore;
                    result.DealerScore = Gamelog.DealerScore;
                    result.BalanceChange = Gamelog.BalanceChange;
                    result.Outcome = Gamelog.Outcome;
                    result.Decision = Gamelog.Decision;

                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Removes an gamlelog entry in the database
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            context.Remove(GetAllGamelogs()[index]);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets the win (and push and loss) -percentages for a provided user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (float, float, float) GetWinPct(User user)
        {
            int wins = 0;
            int pushes = 0;
            int losses = 0;
            //GetGamelogs(user).Where(x => x.PlayerCards.Any()).ToList();
            List<Gamelog> gamelogs = new();
            gamelogs = GetGamelogs(user);
            foreach (Gamelog gamelog in gamelogs)
            {
                switch (gamelog.Outcome)
                {
                    case "Win":
                        wins++;
                        break;

                    case "Push":
                        pushes++;
                        break;

                    case "Loss":
                        losses++;
                        break;

                    default:
                        pushes++;
                        break;
                }
            }

            return PctCalculator(wins, pushes, losses, gamelogs.Count);
        }

        /// <summary>
        /// Calculates percentages from three floats and returns  a tuple as result
        /// </summary>
        /// <param name="wins"></param>
        /// <param name="pushes"></param>
        /// <param name="losses"></param>
        /// <returns></returns>
        public (float, float, float) PctCalculator(float wins, float pushes, float losses, int count)
        {
            (float, float, float) result;

            result = new(
                (float)wins / (wins + losses) * 100f,
                (float)pushes / count * 100f,
                (float)losses / (wins + losses) * 100f);
            return result;
        }

        /// <summary>
        /// Adds a gamlelog to database
        /// </summary>
        /// <param name="Gamelog"></param>
        public void Add(Gamelog Gamelog)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbContext>();
            using GameDbContext context = new GameDbContext(optionsBuilder.Options);
            context.Update(Gamelog);

            context.SaveChanges();
        }

        /// <summary>
        /// Creates a gamelog and calls the add to database function
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="PlayerCards"></param>
        /// <param name="DealerCards"></param>
        /// <param name="PlayerScore"></param>
        /// <param name="DealerScore"></param>
        /// <param name="BalanceChange"></param>
        /// <param name="Decision"></param>
        /// <param name="PlayerWin"></param>
        public void NewLog(int userID, string PlayerCards, string DealerCards, int PlayerScore, int DealerScore, float BalanceChange, bool Decision, string PlayerWin)
        {
            Gamelog gamelog = new Gamelog(userID, PlayerCards, DealerCards, PlayerScore, DealerScore, BalanceChange, Decision, PlayerWin);

            Add(gamelog);
        }
    }
}