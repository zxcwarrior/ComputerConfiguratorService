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
using ComputerConfiguratorService.Utilities;

namespace ComputerConfiguratorService.View
{
    /// <summary>
    /// Логика взаимодействия для ComponentsPage.xaml
    /// </summary>
    public partial class ComponentsPage : Page
    {
        public ComponentsPage()
        {
            InitializeComponent();
        }
        private void CPUsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new CPUsPage());
        }

        private void GPUsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new GPUsPage());
        }

        private void RAMsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RAMsPage());
        }

        private void StoragesButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new StoragesPage());
        }

        private void PowerSuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PowerSuppliesPage());
        }

        private void MotherboardsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new MotherboardsPage());
        }

        private void CPUCoolingButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new CPUCoolingPage());
        }

        private void CasesButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new CasesPage());
        }

        private void CaseCoolingButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new CaseCoolingPage());
        }
    }
}
