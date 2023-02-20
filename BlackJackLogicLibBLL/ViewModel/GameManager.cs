/*---------------------------------
 *  Programming in C# III
 *  Assignment 4
 *  By Alfred Geraldsson
 * --------------------------------
 */

using BlackJackLogicBLL.ViewModel;
using BlackJackLogicBLL.ViewModel.Enums;
using GamelogLib;
using Microsoft.EntityFrameworkCore;
using ModelsEL;
using System;
using System.Collections.Generic;

namespace BlackJackLogicBLL

{
    /// <summary>
    /// Manages all gamelogic
    ///
    ///     1. Constructor: Players are set up and one intial deck is shuffled.
    ///     2. When the player clicks "Deal" => Deal(); is called.
    ///
    /// </summary>
    public class GameManager
    {
        private GameOutput output = new();
        private GameState gameState = GameState.GameOver;
        private User ActiveUser = new();
        private Player player;
        private Player dealer;
        private int numberOfDecksRequest = 1;
        private bool setUpNewRound = true;
        private int bet;
        private int balancePreBet;
        private DeckManager deckManager = new();
        private UserManager userManager = new();
        private GamelogManager gamelogManager = new();
        private GameOutcome outcome;

        /// <summary>
        /// Makes sure a user exists and the deck is shuffled at creation
        /// </summary>
        public GameManager()
        {
            if (dealer == null) dealer = new();
            if (player == null) player = new();

            SetPlayer();
            output.Balance = ActiveUser.Balance;
            ShuffleCards(1);
        }

        /// <summary>
        /// Contains all logic for seting up a new round
        /// </summary>
        /// <param name="autoShuffle"></param>
        public void Deal(bool autoShuffle, int bet)
        {
            balancePreBet = ActiveUser.Balance; ;
            player.State = PlayerState.Active;
            dealer.State = PlayerState.Active;
            if (autoShuffle) CheckAutoShuffle();
            if (setUpNewRound)
            {
                ShuffleCards(numberOfDecksRequest);
                setUpNewRound = false;
            }

            this.bet = bet;
            ActiveUser.Balance = ActiveUser.Balance - bet;
            dealer.Hand.Clear();
            player.Hand.Clear();
            dealer.AddCard(deckManager.DrawCard());
            player.AddCard(deckManager.DrawCard());
            player.AddCard(deckManager.DrawCard());
            gameState = GameState.PlayerTurn;
            output.Balance = ActiveUser.Balance;
            output.PlayerGraphic = "neutral";
            output.DealerGraphic = "neutral";

            UpdateUser();
            DetermineGameState();
        }

        /// <summary>
        /// Main game cycle. Determines the player and dealer states.
        /// Updates the corresponding output
        /// </summary>
        private void RunGameCycle()
        {
            output.DealerScore = dealer.Hand.HandValueTotal().ToString();
            output.PlayerScore = player.Hand.HandValueTotal().ToString();
            output.DealerCards = dealer.Hand.Cards;
            output.PlayerCards = player.Hand.Cards;
            output.CardsLeft = deckManager.Deck.Count.ToString();

            DetermineBust();

            if (player.State == PlayerState.Standing || player.State == PlayerState.BlackJack)
            {
                DeterminePush();
                DetermineWin();
                DetermineLoss();
            }

            if (gameState == GameState.GameOver)
            {
                DetermineWinnings();
                CreateGamelog();
                UpdateUser();
            }
            SetButtonsToShow();
        }

        /// <summary>
        /// Sets an default active user and some inital values
        /// </summary>
        public void SetPlayer()
        {
            gamelogManager.RemoveUserLogs(0);
            User defaultUser = new User("NewPlayer", 100, 0);
            ActiveUser = defaultUser;
            player.Name = defaultUser.Username;
            player.UserId = defaultUser.Id;
            output.WinPct = 50;
            UpdateUser();
        }

        /// <summary>
        /// Sets a user as the active player
        /// </summary>
        /// <param name="index"></param>
        public void SetPlayer(int index)
        {
            ActiveUser = ConvertindexToUser(index);
            UpdateUser();
        }

        /// <summary>
        /// Sets a user as the active player
        /// </summary>
        /// <param name="user"></param>
        public void SetPlayer(User user)
        {
            ActiveUser = user;

            UpdateUser();
        }

        /// <summary>
        /// Fecthes the initial dbo output object used for the initial game setup
        /// </summary>
        /// <returns></returns>
        public BJSetupDTO InitialOutput_DTO() => output.ApplyInitialUserBlackJackDTO();

        /// <summary>
        /// Fetches the blackjack dto that is applied as the collected dara for the output
        /// </summary>
        /// <returns></returns>
        public BlackJackDTO Output_DTO() => output.ApplyBlackJackDTO();

        /// <summary>
        /// Returns a user at a specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private User ConvertindexToUser(int index)
        {
            return userManager.GetUsers()[index];
        }

        /// <summary>
        /// Updates the active user to database
        /// Updates new output values
        /// </summary>
        private void UpdateUser()
        {
            userManager.Insert(ActiveUser);
            output.Balance = ActiveUser.Balance;
            output.UserName = ActiveUser.Username;
            output.WinPct = gamelogManager.GetWinPct(ActiveUser).Item1;
        }

        /// <summary>
        /// Checks for auto shuffle and auto shuffles if the citeria is met
        /// </summary>
        private void CheckAutoShuffle()
        {
            if (numberOfDecksRequest * 52 / 4 > deckManager.Deck.Count) ShuffleCards(numberOfDecksRequest);
        }

        /// <summary>
        /// Shuffles the deck and sets the last deck request to the provided number
        /// </summary>
        /// <param name="numberOfDecks"></param>
        public void ShuffleCards(int decks)
        {
            numberOfDecksRequest = decks;
            deckManager.SetUpDecks(numberOfDecksRequest);
            output.CardsLeft = deckManager.Deck.Count.ToString();
        }

        /// <summary>
        /// Determines wheter a player is bust or not
        /// </summary>
        private void DetermineBust()
        {
            if (player.State == PlayerState.Bust)
            {
                output.PlayerScore = $"BUST " + $"{player.Hand.HandValueTotal()}";
                output.PlayerGraphic = "loss";
                output.DealerGraphic = "win";
                outcome = GameOutcome.Loss;
                gameState = GameState.GameOver;
            }

            if (dealer.State == PlayerState.Bust)
            {
                output.DealerScore = $"BUST " + $"{dealer.Hand.HandValueTotal()}";
                output.DealerGraphic = "loss";
                output.PlayerGraphic = "win";

                gameState = GameState.GameOver;
            }
        }

        /// <summary>
        /// Determines wheter the players are at a push
        /// </summary>
        private void DeterminePush()
        {
            if (player.Hand.HandValueTotal() == dealer.Hand.HandValueTotal())
            {
                output.PlayerScore = $"PUSH {player.Hand.HandValueTotal()}";
                output.DealerScore = $"PUSH {dealer.Hand.HandValueTotal()}";
                if (player.Hand.HandValueTotal() == 21)
                {
                    output.PlayerScore = $"PUSH 21";
                    output.DealerScore = $"PUSH 21";
                }
                output.PlayerGraphic = "push";
                output.DealerGraphic = "push";

                outcome = GameOutcome.Push;
                gameState = GameState.GameOver;
            }
        }

        /// <summary>
        /// Detrmines wheter the player has won
        /// </summary>
        private void DetermineWin()
        {
            if (player.Hand.HandValueTotal() > dealer.Hand.HandValueTotal() || dealer.State == PlayerState.Bust || dealer.State == PlayerState.TurnOver)
            {
                output.PlayerScore = $"WIN! {player.Hand.HandValueTotal()}";

                if (player.State == PlayerState.BlackJack)
                {
                    output.PlayerScore = $"BlackJack!";
                    outcome = GameOutcome.BlackJack;
                }
                output.DealerGraphic = "loss";
                output.PlayerGraphic = "win";
                outcome = GameOutcome.Win;
                gameState = GameState.GameOver;
            }
        }

        /// <summary>
        /// Determines wheter the player has lost or not
        /// </summary>
        private void DetermineLoss()
        {
            if (player.Hand.HandValueTotal() < dealer.Hand.HandValueTotal() && dealer.State != PlayerState.Bust)
            {
                output.PlayerScore = $"Lose {player.Hand.HandValueTotal()}";
                if (dealer.Hand.HandValueTotal() == 21 && dealer.Hand.Count == 2)
                {
                    output.DealerScore = $"BlackJack!";
                }
                output.PlayerGraphic = "loss";
                output.DealerGraphic = "win";

                outcome = GameOutcome.Loss;
                gameState = GameState.GameOver;
            }
        }

        /// <summary>
        /// Updates the game state depending upon how many points the players hs reached
        /// </summary>
        private void DetermineGameState()
        {
            if (player.Hand.HandValueTotal() >= 22)
            {
                player.State = PlayerState.Bust;
            }

            if (player.Hand.HandValueTotal() == 21)
            {
                player.State = PlayerState.Standing;
                if (player.Hand.Count == 2) player.State = PlayerState.BlackJack;
            }

            if (player.State == PlayerState.BlackJack)
            {
                dealer.AddCard(deckManager.DrawCard());
                if (dealer.Hand.HandValueTotal() == 21) dealer.State = PlayerState.BlackJack;
                else dealer.State = PlayerState.TurnOver;
            }

            if (player.State == PlayerState.Standing && dealer.State != PlayerState.Standing)
            {
                while (dealer.Hand.HandValueTotal() <= 16)
                {
                    dealer.AddCard(deckManager.DrawCard());
                }
                if (dealer.Hand.HandValueTotal() >= 22)
                {
                    dealer.State = PlayerState.Bust;
                }
            }

            RunGameCycle();
        }

        /// <summary>
        /// Updates which buttons are to be shown
        /// </summary>
        private void SetButtonsToShow()
        {
            switch (gameState)
            {
                case GameState.GameOver:
                    output.BtnVisibleShuffle = true;
                    output.BtnVisibleDeal = true;
                    output.BtnVisibleHit = false;
                    output.BtnVisibleStand = false;
                    break;

                case GameState.PlayerTurn:
                    output.BtnVisibleShuffle = false;
                    output.BtnVisibleDeal = false;
                    output.BtnVisibleHit = true;
                    output.BtnVisibleStand = true;
                    output.DealerGraphic = "neutral";
                    output.PlayerGraphic = "neutral";

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Determines what the player has won based on the bet
        /// </summary>
        private void DetermineWinnings()
        {
            switch (outcome)
            {
                case GameOutcome.Win:
                    ActiveUser.Balance = ActiveUser.Balance + (bet * 2);
                    break;

                case GameOutcome.BlackJack:
                    ActiveUser.Balance = ActiveUser.Balance + (bet * 2) + (bet / 2);
                    break;

                case GameOutcome.Push:
                    ActiveUser.Balance = ActiveUser.Balance + bet;
                    break;

                case GameOutcome.Loss:
                    break;

                default:
                    break;
            }
            UpdateUser();
        }

        /// <summary>
        /// Contains logic for the Hit option
        /// </summary>
        public void Hit()
        {
            if (player.State == PlayerState.Active)
            {
                player.AddCard(deckManager.DrawCard());
            }
            DetermineGameState();
        }

        /// <summary>
        /// Contains logic for the Stand option
        /// </summary>
        public void Stand()
        {
            if (player.State != PlayerState.Bust)
            {
                player.State = PlayerState.Standing;
            }
            DetermineGameState();
        }

        /// <summary>
        /// Creates a gamelog and sens it to the gamelog manager to be saved to database
        /// </summary>
        private void CreateGamelog()
        {
            string PlayerCards;
            string DealerCards;
            int PlayerScore;
            int DealerScore;
            float BalanceChange;
            bool Decision = true;
            string PlayerWin;

            PlayerCards = player.Hand.ToString();
            DealerCards = dealer.Hand.ToString();
            PlayerScore = player.Hand.HandValueTotal();
            DealerScore = dealer.Hand.HandValueTotal();
            BalanceChange = ActiveUser.Balance - balancePreBet;
            switch (outcome)
            {
                case GameOutcome.Win:
                    PlayerWin = "Win";
                    break;

                case GameOutcome.BlackJack:
                    PlayerWin = "Win";
                    break;

                case GameOutcome.Loss:
                    PlayerWin = "Loss";
                    break;

                case GameOutcome.Push:
                    PlayerWin = "Push";
                    break;

                default:
                    PlayerWin = "N/A";
                    break;
            }

            gamelogManager.NewLog(
                ActiveUser.Id,
                PlayerCards,
                DealerCards,
                PlayerScore,
                DealerScore,
                BalanceChange,
                Decision,
                PlayerWin);
        }

        /// <summary>
        /// Adds a user taking a name parameter
        /// </summary>
        /// <param name="text"></param>
        public void AddUser(string text)
        {
            User user = new(text, 1000, 0);
            userManager.Add(user);
        }

        public void ChangeUser(int selectedIndex, string newName)
        {
            ActiveUser.Username = newName;
            User user = new();
            userManager.Insert(ActiveUser);
        }

        public void DeleteUser(int selectedIndex)
        {
            userManager.Remove(selectedIndex);
        }

        /// <summary>
        /// Returns the list of users in the database
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            return userManager.GetUsers();
        }

        public User SearchUser(string search)
        {
            return userManager.SearchUser(search);
        }
    }
}