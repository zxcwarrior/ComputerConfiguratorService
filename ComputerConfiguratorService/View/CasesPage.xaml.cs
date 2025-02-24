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
    /// Логика взаимодействия для CasesPage.xaml
    /// </summary>
    public partial class CasesPage : Page
    {
        private Cases selectedCase = null;
        public CasesPage()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadCases();
        }
        private void LoadComboBoxes()
        {
            var context = DatabaseEntities.GetContext();
            cbManufacturer.ItemsSource = context.Manufacturers.ToList();
            cbCaseFF.ItemsSource = context.CaseFormFactors.ToList();
        }
        private void LoadCases()
        {
            LVCases.ItemsSource = DatabaseEntities.GetContext().Cases.ToList();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVCases.SelectedItem is Cases cs)
            {
                selectedCase = cs;
                cbManufacturer.SelectedValue = cs.ManufacturerID;
                cbCaseFF.SelectedValue = cs.CaseFFID;
                tbModel.Text = cs.Model;
                tbMaxGPULength.Text = cs.MaxGPULength.ToString();
                tbMaxCoolers.Text = cs.MaxCoolers.ToString();
                tbPrice.Text = cs.Price.ToString();
                tbImagePath.Text = cs.ImagePath;
            }
            else
            {
                selectedCase = null;
                ClearForm();
            }
            EditPanel.Visibility = Visibility.Visible;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbManufacturer.SelectedValue == null || cbCaseFF.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производителя и форм-фактор.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                int manufacturerID = (int)cbManufacturer.SelectedValue;
                int caseFFID = (int)cbCaseFF.SelectedValue;
                string model = tbModel.Text;
                int maxGPULength = int.Parse(tbMaxGPULength.Text);
                int maxCoolers = int.Parse(tbMaxCoolers.Text);
                decimal price = decimal.Parse(tbPrice.Text);
                string imagePath = tbImagePath.Text;
                var context = DatabaseEntities.GetContext();
                if (selectedCase == null)
                {
                    Cases newCase = new Cases
                    {
                        ManufacturerID = manufacturerID,
                        CaseFFID = caseFFID,
                        Model = model,
                        MaxGPULength = maxGPULength,
                        MaxCoolers = maxCoolers,
                        Price = price,
                        ImagePath = imagePath
                    };
                    context.Cases.Add(newCase);
                }
                else
                {
                    selectedCase.ManufacturerID = manufacturerID;
                    selectedCase.CaseFFID = caseFFID;
                    selectedCase.Model = model;
                    selectedCase.MaxGPULength = maxGPULength;
                    selectedCase.MaxCoolers = maxCoolers;
                    selectedCase.Price = price;
                    selectedCase.ImagePath = imagePath;
                }
                context.SaveChanges();
                MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadCases();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (LVCases.SelectedItem is Cases cs)
            {
                if (MessageBox.Show("Удалить выбранный корпус?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = DatabaseEntities.GetContext();
                        context.Cases.Remove(cs);
                        context.SaveChanges();
                        MessageBox.Show("Корпус удалён.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCases();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите корпус для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
            cbCaseFF.SelectedIndex = -1;
            tbModel.Text = "";
            tbMaxGPULength.Text = "";
            tbMaxCoolers.Text = "";
            tbPrice.Text = "";
            tbImagePath.Text = "";
            selectedCase = null;
        }
    }
}
