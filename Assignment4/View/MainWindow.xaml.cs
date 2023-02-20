using Assignment4.View;
using BlackJackLogicBLL;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Assignment4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Graphics graphics = new();
        private GameManager gameManager = new();

        /// <summary>
        /// Initializes and sets up some inital values
        /// </summary>
        public MainWindow()
        {
            DataContext = graphics;
            InitialGraphicsUpdate();
            InitializeComponent();
        }

        private void InitialGraphicsUpdate() => graphics.Setup(gameManager.InitialOutput_DTO());

        private void UpdateGraphics() => graphics.ApplyBlackJackDTO(gameManager.Output_DTO());

        public event PropertyChangedEventHandler? PropertyChanged;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        /// <summary>
        /// Handles stand button logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStand_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Stand();
            UpdateGraphics();
        }

        /// <summary>
        /// Initiates the game cycle.
        /// Checks if the bet value is valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;
            if (txtBet.Text != "")
            {
                bool validBet = int.TryParse(txtBet.Text, out result);
                if (!validBet)
                {
                    MessageBox.Show("Please enter a valid amount");
                }
            }
            else txtBet.Text = "0";
            gameManager.Deal((bool)chkAutoShuffle.IsChecked, result);
            UpdateGraphics();
        }

        /// <summary>
        /// Handles hit button logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Hit();
            UpdateGraphics();
        }

        /// <summary>
        /// Checks the input before passing on the amount of decks that are to be shuffled,
        /// Displays an error message if the wrong input is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            bool decks = int.TryParse(txtNoDecks.Text, out int result);
            //if (!decks) DisplayAlert("Error", "Please enter a valid number", "OK");
            gameManager.ShuffleCards(result);
            graphics.Setup(gameManager.InitialOutput_DTO());
            InitialGraphicsUpdate();
        }

        /// <summary>
        /// Shows the change user window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            UserWindow window = new UserWindow(gameManager);
            window.ShowDialog();
            InitialGraphicsUpdate();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_FullScreen_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}