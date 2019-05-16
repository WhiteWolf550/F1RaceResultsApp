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
    /// Interakční logika pro QualifyingPage.xaml
    /// </summary>
    public partial class QualifyingPage : Page {
        private Frame frame;
        static private JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        quali.Qualifying details = new quali.Qualifying();
        public QualifyingPage() {
            InitializeComponent();
            GetDetails();
        }

        public QualifyingPage(Frame parentFrame) : this() {
            frame = parentFrame;

        }

        public async void GetDetails() {

            HttpClient httpclient = new HttpClient();
            var response = await httpclient.GetAsync("https://ergast.com/api/f1/current/last/qualifying.json?");

            string content = await response.Content.ReadAsStringAsync();
            details = JsonConvert.DeserializeObject<quali.Qualifying>(content, settings);

            Load();



        }

        public void Load() {
            int size = 100;
            foreach (quali.Race race in details.MRData.RaceTable.Races) {
                ResultType.Content = race.season + " " + race.Circuit.circuitName;
                foreach (quali.QualifyingResult result in race.QualifyingResults) {
                    int pos = Int32.Parse(result.position);
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    stack.Background = GetColor(pos);
                    Label position = new Label();
                    Rectangle teamcolor = new Rectangle();
                    Label number = new Label();
                    Label drivername = new Label();
                    Label q1 = new Label();
                    Label q2 = new Label();
                    Label q3 = new Label();



                    teamcolor.Width = 10;
                    teamcolor.Fill = GetTeamColor(result.Constructor.name);
                    teamcolor.ToolTip = result.Constructor.name;
                    position.Content = result.position;
                    position.Width = size;
                    number.Content = result.number;
                    number.Width = size;
                    drivername.Content = result.Driver.givenName + " " + result.Driver.familyName;
                    drivername.Width = size;
                    q1.Content = result.Q1;
                    q1.Width = size;
                    q1.HorizontalContentAlignment = HorizontalAlignment.Center;
                    q2.Content = result.Q2;
                    q2.Width = size;
                    q2.HorizontalContentAlignment = HorizontalAlignment.Center;
                    q3.Content = result.Q3;
                    q3.Width = size;
                    q3.HorizontalContentAlignment = HorizontalAlignment.Center;

                    stack.Children.Add(teamcolor);
                    stack.Children.Add(position);
                    stack.Children.Add(number);
                    stack.Children.Add(drivername);
                    stack.Children.Add(q1);
                    stack.Children.Add(q2);
                    stack.Children.Add(q3);


                    ResultPanel.Children.Add(stack);
                }
            }
        }

        public SolidColorBrush GetColor(int position) {
            if (position == 1) {
                return Brushes.Gold;
            } else if (position == 2) {
                return Brushes.Silver;
            } else if (position == 3) {
                return Brushes.SandyBrown;
            } else {
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

        private void Race_Click(object sender, RoutedEventArgs e) {
            frame.Navigate(new Results(frame));
        }
    }
}
