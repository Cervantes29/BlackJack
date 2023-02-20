using BlackJackLogicBLL.ViewModel.Enums;
using ModelsEL;

namespace BlackJackLogicBLL

{
    /// <summary>
    /// Main object to pass through all gamelogic. Represent both player and dealer.
    /// </summary>
    public class Player
    {
        public Player(User user) : this()
        {
            Currency = user.Balance;
            Name = user.Username;
        }

        public Player()
        {
            Hand = new Hand();
            State = PlayerState.Active;
            Currency = 10000;
            Name = "No name";
        }

        public PlayerState State { get; set; }
        private int currency;
        private int userId;
        private Hand hand;
        private string name;
        public Hand Hand { get => hand; set => hand = value; }
        public int Currency { get => currency; set => currency = value; }
        public string Name { get => name; set => name = value; }
        public int UserId { get => userId; set => userId = value; }

        /// <summary>
        /// Adds a card by sending it to the add card in the hand class
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card) => Hand.Add(card);

        /// <summary>
        /// Removes a card by sending it to the renove card function of the hand class
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(Card card) => Hand.Remove(card);
    }
}