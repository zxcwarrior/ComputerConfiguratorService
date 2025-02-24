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
using ComputerConfiguratorService.Model;

namespace ComputerConfiguratorService.View
{
    /// <summary>
    /// Логика взаимодействия для CaseCoolingPage.xaml
    /// </summary>
    public partial class CaseCoolingPage : Page
    {
        private CaseCooling selectedCaseCooling = null;
        public CaseCoolingPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadCaseCooling();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
        }
        private void LoadCaseCooling()
        {
            LVCaseCooling.ItemsSource = DatabaseEntities.GetContext().CaseCooling.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVCaseCooling.SelectedItem is CaseCooling cc)
            {
                selectedCaseCooling = cc;
                cbManufacturer.SelectedValue = cc.ManufacturerID;
                tbModel.Text = cc.Model;
                tbFanSize.Text = cc.FanSize.ToString();
                tbPrice.Text = cc.Price.ToString();
            }
            else
            {
                selectedCaseCooling = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                string model = tbModel.Text;
                int fanSize = int.Parse(tbFanSize.Text);
                decimal price = decimal.Parse(tbPrice.Text);
                var context = DatabaseEntities.GetContext();
                if (selectedCaseCooling == null)
                {
                    CaseCooling newCC = new CaseCooling
                    {
                        ManufacturerID = manufacturerID,
                        Model = model,
                        FanSize = fanSize,
                        Price = price
                    };
                    context.CaseCooling.Add(newCC);
                }
                else
                {
                    selectedCaseCooling.ManufacturerID = manufacturerID;
                    selectedCaseCooling.Model = model;
                    selectedCaseCooling.FanSize = fanSize;
                    selectedCaseCooling.Price = price;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadCaseCooling();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVCaseCooling.SelectedItem is CaseCooling cc)
            {
                if (MessageBox.Show("Удалить выбранное охлаждение корпуса?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.CaseCooling.Remove(cc);
                        context.SaveChanges();
                        MessageBox.Show("Охлаждение корпуса удалено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCaseCooling();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите охлаждение корпуса для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            EditPanel.Visibility = Visibility.Collapsed;
        }
        private void ClearForm()
        {
            cbManufacturer.SelectedIndex = -1;
            tbModel.Text = "";
            tbFanSize.Text = "";
            tbPrice.Text = "";
            selectedCaseCooling = null;
        }
    }
}
