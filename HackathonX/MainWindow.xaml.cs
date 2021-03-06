using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Rectangle = System.Windows.Shapes.Rectangle;
using Brushes = System.Windows.Media.Brushes;
using System.Drawing.Imaging;
using HackathonX.DB.Model;
using System.ComponentModel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using HackathonX.DB.Repositories;

namespace HackathonX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DbContextOptions<HackathonXContext> _contextOptions;
        private readonly HackathonXContext _hackathonXContext;

        public IEnumerable<Question> Questions = new List<Question>();

        private readonly SqliteConnection _connection;

        public MainWindow()
        {
            _connection = new SqliteConnection("Data Source=HackathonX.db;");//new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<HackathonXContext>()
                .UseSqlite(_connection)
                .Options;

            _hackathonXContext = new HackathonXContext(_contextOptions);

            using var questionnaireRepository = new QuestionnaireRepository(_hackathonXContext);
            IEnumerable<Question> result =  questionnaireRepository.GetQuestionnaire(3).GetAwaiter().GetResult();

            InitializeComponent();
            base.Loaded += Window1_Loaded;
            DataContext = CurrentUser;
        }

        private string image = @"images/world.png";
        private ImageBrush ib;
        private Rectangle[] imagepieces = new Rectangle[9];

        private const double PiecesInterval = 0.33;
        private const double PiecesWidth = 0.33;
        private const double PiecesHeight = 0.33;
        BitmapImage myBitmapImage = new BitmapImage();

        private int _CurrentScore;

        public User CurrentUser { get; set; }

        public int CurrentScore {
            get
            {
                return _CurrentScore;
            }
            set 
            {
                _CurrentScore = value;
                NotifyPropertyChanged("CurrentScore");
            }
        }

        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.DynamicInvoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }

        private void LoadImage()
        {
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(image, UriKind.Relative);
            myBitmapImage.EndInit();

            int counter = 0;
            double startTop = 0;
            double startLeft = 0;

            while (startTop < 0.99)
            {
                startLeft = 0;
                while (startLeft < 0.99)
                {
                    imagepieces[counter] = CreatePieces(startTop, startLeft);
                    startLeft = startLeft + PiecesInterval;
                    counter = counter + 1;
                }
                startTop = startTop + PiecesInterval;
            }

            counter = 0;

            int gridrow;
            int gridcolumn;
            for (gridcolumn = 0; gridcolumn < 3; gridcolumn++)
            {
                for (gridrow = 0; gridrow < 3; gridrow++)
                {
                    Border imageBorder = new Border();
                    imagepieces[counter].Opacity = 0.03;
                    imageBorder.BorderBrush = Brushes.Black;
                    imageBorder.BorderThickness = new Thickness(0.5);
                    imageBorder.SetValue(Grid.RowProperty, gridrow);
                    imageBorder.SetValue(Grid.ColumnProperty, gridcolumn);
                    imageBorder.Child = imagepieces[counter];

                    ImageGrid.Children.Add(imageBorder);
                    counter = counter + 1;
                }
            }
        }

        private Rectangle CreatePieces(double top, double left)
        {
            ib = new ImageBrush();
            ib.ImageSource = myBitmapImage;

            ib.Viewbox = new Rect(top, left, PiecesWidth, PiecesHeight);
            ib.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            ib.TileMode = TileMode.None;
            var rec = new Rectangle();
            rec.Stretch = Stretch.Fill;
            rec.Fill = ib;
            return rec;
        }

    }
}
