using Humanizer.Localisation;
using ModelsEL;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

//using BlackJackLogicLib_BLL.ViewModel;

namespace Assignment4
{
    /// <summary>
    /// Contains all dynamic collections that are to be show in the program such as card hands and points.
    /// All observable collections and properties are for notifiying the xaml code that something has changed.
    /// </summary>
    public class Graphics : INotifyPropertyChanged
    {
        private ObservableCollection<Card> playerCards = new();
        private ObservableCollection<Card> dealerCards = new();
        private string dealerScore;
        private Brush dealerState;
        private string playerScore;
        private Brush playerState;
        private string cardsLeft;
        private bool inbetweenRounds = true;
        private bool btnVisibleStand;
        private bool btnVisibleHit;
        private bool btnVisibleShuffle = true;
        private string lblBalance = "0";
        private string playerName = "No User";
        private float winPct = 50;
        private LinearGradientBrush gradientBrushWin = new();
        private LinearGradientBrush gradientBrushLoss = new();
        private LinearGradientBrush gradientBrushNeutral = new();
        private LinearGradientBrush gradientBrushPush = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Graphics() => SetUpAllGradients();

        private void SetUpAllGradients()
        {
            gradientBrushWin.GradientStops.Add(new GradientStop(Color.FromRgb(127, 187, 179), 0));
            gradientBrushWin.GradientStops.Add(new GradientStop(Color.FromRgb(75, 124, 128), 1));
            gradientBrushWin.StartPoint = new Point(1, 0);
            gradientBrushWin.EndPoint = new Point(1, 1);

            gradientBrushLoss.GradientStops.Add(new GradientStop(Color.FromRgb(231, 125, 160), 0));
            gradientBrushLoss.GradientStops.Add(new GradientStop(Color.FromRgb(149, 79, 93), 1));
            gradientBrushLoss.StartPoint = new Point(1, 0);
            gradientBrushLoss.EndPoint = new Point(1, 1);

            gradientBrushPush.GradientStops.Add(new GradientStop(Color.FromRgb(231, 214, 184), 0));
            gradientBrushPush.GradientStops.Add(new GradientStop(Color.FromRgb(160, 125, 111), 1));
            gradientBrushPush.StartPoint = new Point(1, 0);
            gradientBrushPush.EndPoint = new Point(1, 1);

            gradientBrushNeutral.GradientStops.Add(new GradientStop(Color.FromArgb(255, 255, 230, 250), 0));
            gradientBrushNeutral.GradientStops.Add(new GradientStop(Color.FromArgb(255, 243, 234, 248), 1));
            gradientBrushNeutral.StartPoint = new Point(1, 0);
            gradientBrushNeutral.EndPoint = new Point(1, 1);
        }

        /// <summary>
        /// All graphics are updated through applying one DTO throughthis method
        /// </summary>
        /// <param name="dto"></param>
        public void ApplyBlackJackDTO(BlackJackDTO dto)
        {
            if (dto != null)
            {
                DealerCards = new ObservableCollection<Card>(dto.PlayerCards);
                PlayerCards = new ObservableCollection<Card>(dto.DealerCards);
                InbetweenRounds = dto.InbetweenRounds;
                BtnVisibleHit = dto.HitAvailable;
                BtnVisibleShuffle = dto.ShuffleAvailable;
                BtnVisibleStand = dto.StandAvailalle;
                PlayerScore = dto.PlayerString;
                DealerScore = dto.DealerString;
                switch (dto.DealerCurrentState)
                {
                    case "win":
                        DealerBrush = gradientBrushWin;
                        break;

                    case "loss":
                        DealerBrush = gradientBrushLoss;
                        break;

                    case "push":
                        DealerBrush = gradientBrushPush;
                        break;

                    case "neutral":
                        DealerBrush = gradientBrushNeutral;
                        break;

                    default:
                        DealerBrush = gradientBrushNeutral;

                        break;
                }
                switch (dto.PlayerCurrentState)
                {
                    case "win":
                        PlayerBrush = gradientBrushWin;
                        break;

                    case "loss":
                        PlayerBrush = gradientBrushLoss;
                        break;

                    case "push":
                        PlayerBrush = gradientBrushPush;
                        break;

                    case "neutral":
                        PlayerBrush = gradientBrushNeutral;
                        break;

                    default:
                        PlayerBrush = gradientBrushNeutral;

                        break;
                }
                CardsLeft = dto.CardsLeft;
                PlayerName = dto.UserName;
                LblBalance = dto.Balance.ToString();
                LblWinPct = dto.WinPct;
            }
        }

        /// <summary>
        /// Updates the initial screen graphics of the project
        /// </summary>
        /// <param name="dto"></param>
        internal void Setup(BJSetupDTO dto)
        {
            if (dto != null)
            {
                InbetweenRounds = dto.InbetweenRounds;
                BtnVisibleHit = dto.HitAvailable;
                BtnVisibleShuffle = dto.ShuffleAvailable;
                BtnVisibleStand = dto.StandAvailalle;
                LblBalance = dto.Balance.ToString();
                PlayerName = dto.UserName;
                CardsLeft = dto.CardsLeft;
            }
        }

        public ObservableCollection<Card> PlayerCards
        {
            get => playerCards;
            set
            {
                playerCards = value;
                OnPropertyChanged("PlayerCards");
            }
        }

        public ObservableCollection<Card> DealerCards
        {
            get => dealerCards;
            set
            {
                dealerCards = value;
                OnPropertyChanged("DealerCards");
            }
        }

        public string DealerScore
        {
            get => dealerScore;
            set
            {
                dealerScore = value;
                OnPropertyChanged("DealerScore");
            }
        }

        public string PlayerScore
        {
            get => playerScore;
            set
            {
                playerScore = value;
                OnPropertyChanged("PlayerScore");
            }
        }

        public Brush DealerBrush
        {
            get => dealerState;
            set
            {
                dealerState = value;
                OnPropertyChanged("DealerBrush");
            }
        }

        public Brush PlayerBrush
        {
            get => playerState;
            set
            {
                playerState = value;
                OnPropertyChanged("PlayerBrush");
            }
        }

        public string CardsLeft
        {
            get => cardsLeft;
            set
            {
                cardsLeft = value;
                OnPropertyChanged("CardsLeft");
            }
        }

        public bool InbetweenRounds
        {
            get => inbetweenRounds;
            set
            {
                inbetweenRounds = value;
                OnPropertyChanged("InbetweenRounds");
            }
        }

        public bool BtnVisibleStand
        {
            get => btnVisibleStand;
            set
            {
                btnVisibleStand = value;
                OnPropertyChanged("BtnVisibleStand");
            }
        }

        public bool BtnVisibleHit
        {
            get => btnVisibleHit;
            set
            {
                btnVisibleHit = value;
                OnPropertyChanged("BtnVisibleHit");
            }
        }

        public bool BtnVisibleShuffle
        {
            get => btnVisibleShuffle;
            set
            {
                btnVisibleShuffle = value;
                OnPropertyChanged("BtnVisibleShuffle");
            }
        }

        public string LblBalance
        {
            get => lblBalance;
            set
            {
                lblBalance = value;
                OnPropertyChanged("LblBalance");
            }
        }

        public string PlayerName
        {
            get => playerName;
            set
            {
                playerName = value;
                OnPropertyChanged("PlayerName");
            }
        }

        public float LblWinPct
        {
            get => winPct;
            set
            {
                winPct = value;
                OnPropertyChanged("LblWinPct");
            }
        }
    }
}