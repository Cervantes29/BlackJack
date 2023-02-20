using BlackJackLogicBLL;
using BlackJackLogicBLL.ViewModel;
using ModelsEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Assignment4.View
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private List<User> users = new();
        private ObservableCollection<string> userNames = new();
        private GameManager gameManager;

        public UserWindow(GameManager gameManager)
        {
            DataContext = this;
            InitializeComponent();
            this.gameManager = gameManager;
            Update();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        public List<User> Users
        {
            get => users;
            set
            {
                users = value;
            }
        }

        public ObservableCollection<string> UserStrings
        {
            get => this.userNames;
            set => this.userNames = value;
        }

        /// <summary>
        /// Select and closes the user selection window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lstUsers.SelectedItem != null)
            {
                gameManager.SetPlayer(lstUsers.SelectedIndex);
                Update();
                Close();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            gameManager.AddUser(txtNewUserName.Text);
            Update();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            gameManager.DeleteUser(lstUsers.SelectedIndex);
            Update();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (lstUsers.SelectedItem != null)
            {
                gameManager.SetPlayer(lstUsers.SelectedIndex);
                gameManager.ChangeUser(lstUsers.SelectedIndex, txtNewUserName.Text);
            }
            Update();
        }

        private void Update()
        {
            //UserStrings.Clear();
            //gameManager.GetUsers().ForEach(u => UserStrings.Add(u.Username));
            Users.Clear();
            Users.AddRange(gameManager.GetUsers());

            lstUsers.Items.Clear();
            foreach (User user in Users)
            {
                lstUsers.Items.Add(user.Username);
            }
        }

        private void btnSearchName_Click(object sender, RoutedEventArgs e)
        {
            User user = new();
            user = gameManager.SearchUser(txtSearchName.Text);
            lstUserSearch.Items.Clear();
            if (user != null)
            {
                lstUserSearch.Items.Add(user.Username);
            }
        }

        private void lstUserSearch_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstUserSearch.SelectedItem != null)
            {
                User user = gameManager.GetUsers().Find(item => item.Username == txtSearchName.Text);
                if (user != null) gameManager.SetPlayer(user);
                Update();
                Close();
            }
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