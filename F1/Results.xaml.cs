using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace F1 {
    /// <summary>
    /// Interakční logika pro Results.xaml
    /// </summary>
    public partial class Results : Page {

        static private JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        private Frame frame;
        Details details = new Details();
        public Results() {
            InitializeComponent();
            GetDetails();
        }

        public Results(Frame parentFrame) : this() {
            frame = parentFrame;
        }

        public async void GetDetails() {

            HttpClient httpclient = new HttpClient();
            var response = await httpclient.GetAsync("https://ergast.com/api/f1/current/last/results.json?");

            string content = await response.Content.ReadAsStringAsync();
            details = JsonConvert.DeserializeObject<Details>(content, settings);

            Load();



        }
        public void Load() {
            int size = 100;
            foreach(Race race in details.MRData.RaceTable.Races) {
                ResultType.Content = race.season + " " +  race.Circuit.circuitName;
                foreach(Result result in race.Results) {
                    int pos = Int32.Parse(result.position);
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    stack.Background = GetColor(pos);
                    Label position = new Label();
                    Rectangle teamcolor = new Rectangle();
                    Label number = new Label();
                    Label drivername = new Label();
                    Label grid = new Label();
                    Label time = new Label();
                    Label points = new Label();
                    teamcolor.Width = 10;              
                    teamcolor.Fill = GetTeamColor(result.Constructor.name);
                    teamcolor.ToolTip = result.Constructor.name;
                    position.Content = result.position;
                    position.Width = size;
                    number.Content = result.number;
                    number.Width = size;
                    drivername.Content = result.Driver.givenName + " " + result.Driver.familyName;
                    drivername.Width = size;
                    grid.Content = result.grid;
                    grid.Width = size;
                    //time.Content = result.Time.time;
                    points.Content = result.points;
                    points.Width = size;


                    stack.Children.Add(teamcolor);
                    stack.Children.Add(position);
                    stack.Children.Add(number);
                    stack.Children.Add(drivername);
                    stack.Children.Add(grid);
                    stack.Children.Add(time);
                    stack.Children.Add(points);

                    ResultPanel.Children.Add(stack);
                }
            }
        }
        public SolidColorBrush GetColor(int position) {
            if (position == 1) {
                return Brushes.Gold;
            }else if (position == 2) {
                return Brushes.Silver;
            }else if (position == 3) {
                return Brushes.SandyBrown;
            }else {
                return Brushes.WhiteSmoke;
            }
        }
        public SolidColorBrush GetTeamColor(string team) {
            if (team == "Mercedes") {
                return Brushes.Cyan;
            } else if (team == "Ferrari") {
                return Brushes.Red;
            } else if (team == "Red Bull") {
                return Brushes.DarkBlue;
            } else if (team == "Haas F1 Team") {
                return Brushes.Black;
            } else if (team == "McLaren") {
                return Brushes.Orange;
            } else if (team == "Renault") {
                return Brushes.Yellow;
            } else if (team == "Toro Rosso") {
                return Brushes.Blue;
            } else if (team == "Racing Point") {
                return Brushes.HotPink;
            } else if (team == "Alfa Romeo") {
                return Brushes.DarkRed;
            } else if (team == "Williams") {
                return Brushes.LightBlue;
            } else {
                return Brushes.White;
            }
        }

        private void Qualifying_Click(object sender, RoutedEventArgs e) {
            frame.Navigate(new QualifyingPage(frame));
        }
    }
}
