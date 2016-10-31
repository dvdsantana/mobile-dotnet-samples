using Carto.Ui;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CartoMap.Windows.Pages.Map
{
    public partial class MapBasePage : Page
    {
        protected MapView MapView { get; set; }

        public MapBasePage()
        {
            InitializeComponent();

            MapView = new MapView();

            Window.Current.Content = MapView;
        }
    }
}
