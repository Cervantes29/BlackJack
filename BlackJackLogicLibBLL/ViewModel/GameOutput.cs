using ModelsEL;
using System.Collections.Generic;

namespace BlackJackLogicBLL;

/// <summary>
/// Contains all dynamic collections that are to be show in the program such as card hands and points.
/// All observable collections and properties are for notifiying the xaml code that something has changed.
/// </summary>
public class GameOutput
{
    private BlackJackDTO blackJack_Dto = new();
    private BJSetupDTO setup_Dto = new();
    private List<Card> playerCards;
    private List<Card> dealerCards;
    private string dealerScore;
    private string dealerState;
    private string playerScore;
    private string playerState;
    private string cardsLeft;
    private bool btnVisibleDeal = true;
    private bool btnVisibleStand;
    private bool btnVisibleHit;
    private bool btnVisibleShuffle = true;
    private int balance;
    private float winPct;
    private string userName = "No name set";
    private List<User> users;

    public bool BtnVisibleDeal { get => btnVisibleDeal; set => btnVisibleDeal = value; }
    public bool BtnVisibleStand { get => btnVisibleStand; set => btnVisibleStand = value; }
    public bool BtnVisibleHit { get => btnVisibleHit; set => btnVisibleHit = value; }
    public bool BtnVisibleShuffle { get => btnVisibleShuffle; set => btnVisibleShuffle = value; }
    public List<Card> PlayerCards { get => playerCards; set => playerCards = value; }
    public List<Card> DealerCards { get => dealerCards; set => dealerCards = value; }
    public string DealerScore { get => dealerScore; set => dealerScore = value; }
    public string PlayerScore { get => playerScore; set => playerScore = value; }
    public string CardsLeft { get => cardsLeft; set => cardsLeft = value; }
    public int Balance { get => balance; set => balance = value; }
    public float WinPct { get => winPct; set => winPct = value; }
    public string UserName { get => userName; set => userName = value; }
    public List<User> Users { get => users; set => users = value; }
    public string DealerGraphic { get => dealerState; set => dealerState = value; }
    public string PlayerGraphic { get => playerState; set => playerState = value; }

    internal BlackJackDTO ApplyBlackJackDTO()
    {
        blackJack_Dto.UserName = UserName;
        blackJack_Dto.PlayerCards = PlayerCards;
        blackJack_Dto.DealerCards = DealerCards;
        blackJack_Dto.ShuffleAvailable = BtnVisibleShuffle;
        blackJack_Dto.InbetweenRounds = BtnVisibleDeal;
        blackJack_Dto.HitAvailable = BtnVisibleHit;
        blackJack_Dto.StandAvailalle = BtnVisibleStand;
        blackJack_Dto.DealerString = DealerScore;
        blackJack_Dto.PlayerString = PlayerScore;
        blackJack_Dto.PlayerCurrentState = PlayerGraphic;
        blackJack_Dto.DealerCurrentState = DealerGraphic;
        blackJack_Dto.CardsLeft = CardsLeft;
        blackJack_Dto.Balance = Balance;
        blackJack_Dto.WinPct = WinPct;
        return blackJack_Dto;
    }

    internal BJSetupDTO ApplyInitialUserBlackJackDTO()
    {
        blackJack_Dto.UserName = UserName;
        setup_Dto.ShuffleAvailable = BtnVisibleShuffle;
        setup_Dto.InbetweenRounds = BtnVisibleDeal;
        setup_Dto.HitAvailable = BtnVisibleHit;
        setup_Dto.StandAvailalle = BtnVisibleStand;
        setup_Dto.CardsLeft = CardsLeft;
        setup_Dto.UserName = UserName;
        setup_Dto.Balance = Balance;
        return setup_Dto;
    }
}