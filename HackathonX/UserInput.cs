using HackathonX.DB.Model;
using HackathonX.DB.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Windows;

namespace HackathonX.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserInput : Window
    {
        MainWindow mainWin;
        UserRepository userRepository;
        HackathonXContext dbContext;
        SqliteConnection _connection;


        public UserInput()
        {
            InitializeComponent();


            userRepository = new UserRepository(dbContext);
        }

        private HackathonXContext GetContext()
        {
            _connection = new SqliteConnection("Data Source=HackathonX.db;");
            _connection.Open();

        }


        private void StartPlaying_Click(object sender, RoutedEventArgs e)
        {
            string strName = txtName.Text.Trim();
            Debug.WriteLine($"player: >>>{strName}<<<");

            mainWin = new MainWindow();
            mainWin.CurrentUser = userRepository.GetOrAddUser(strName);

            //User assumeNewUserForNow = new();
            //assumeNewUserForNow.Name = strName;
            //mainWin.CurrentUser = assumeNewUserForNow;
            
            mainWin.Show();

            //5ScoreBoard scoreBoard = new ScoreBoard();
            //5scoreBoard.Show();
            this.Hide();
        }
    }
}
