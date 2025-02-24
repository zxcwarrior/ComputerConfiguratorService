using ComputerConfiguratorService.Utilities;
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
    }
}
