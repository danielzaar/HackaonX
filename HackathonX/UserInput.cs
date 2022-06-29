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

        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<HackathonXContext> _contextOptions;
        private readonly HackathonXContext _hackathonXContext;
        UserRepository userrepo;
       
        public UserInput()
        {
            _connection = new SqliteConnection("Data Source=HackathonX.db;");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<HackathonXContext>()
                .UseSqlite(_connection)
                .Options;

            _hackathonXContext = new HackathonXContext(_contextOptions);

            userrepo = new UserRepository(_hackathonXContext);
            InitializeComponent();
        }

        private void StartPlaying_Click(object sender, RoutedEventArgs e)
        {
            string strName = txtName.Text.Trim();
            Debug.WriteLine($"player: >>>{strName}<<<");
            if (string.IsNullOrEmpty(strName))
            {
                lblFeedback.Content = "Name cannot be empty!";
                return;
            }

            mainWin = new MainWindow();
            User user = userrepo.GetOrAddUser(strName).GetAwaiter().GetResult();

            mainWin.CurrentUser = user;
            
            mainWin.Show();
            
            this.Hide();
        }
    }
}
