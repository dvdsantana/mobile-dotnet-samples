using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CartoMap.Windows.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class LauncherListPage : Page
    {
        public ListBox View { get; set; }

        public LauncherListPage()
        {
            InitializeComponent();


            View = new ListBox();
            View.Background = new SolidColorBrush(Color.FromArgb(255, 240, 240, 240));
            View.Padding = new Thickness(0, 20, 0, 0);
            View.ItemsSource = Samples.List;
            View.ItemTemplate = (DataTemplate)Resources["ListItemTemplate"];
            View.SelectionChanged += OnListItemClick;

            Window.Current.Content = View;
        }

        private void OnListItemClick(object sender, SelectionChangedEventArgs e)
        {
            MapListItem item = (MapListItem)e.AddedItems[0];

            Frame.Navigate(item.Type, item.Name);
        }
    }
}