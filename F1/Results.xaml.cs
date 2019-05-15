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
        Details details = new Details();
        public Results() {
            InitializeComponent();
            GetDetails();
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
                ResultType.Content = race.season + race.Circuit.circuitName;
                foreach(Result result in race.Results) {
                    int pos = Int32.Parse(result.position);
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;
                    stack.Background = GetColor(pos);
                    Label position = new Label();
                    Label number = new Label();
                    Label drivername = new Label();
                    Label grid = new Label();
                    Label time = new Label();
                    Label points = new Label();
                    position.Content = result.position;
                    position.Width = size;
                    number.Content = result.number;
                    number.Width = size;
                    drivername.Content = result.Driver.givenName + result.Driver.familyName;
                    drivername.Width = size;
                    grid.Content = result.grid;
                    grid.Width = size;
                    //time.Content = result.Time.time;
                    points.Content = result.points;
                    points.Width = size;



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
    }
}
