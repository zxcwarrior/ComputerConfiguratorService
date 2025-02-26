using ComputerConfiguratorService.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace ComputerConfiguratorService.View
{
    /// <summary>
    /// Логика взаимодействия для ServiceMenuPage.xaml
    /// </summary>
    public partial class ServiceMenuPage : Page
    {
        public ServiceMenuPage()
        {
            InitializeComponent();
        }
        private void ComponentsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ComponentsPage());
        }

        private void BuildsButton_Click(object sender, RoutedEventArgs e)
        {
            //Manager.MainFrame.Navigate(new BuildsPage());
        }
        private void BackDataButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new BackDataPage());
        }
    }
}
