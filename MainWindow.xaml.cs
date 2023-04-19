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

namespace WpfApp9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SalonEntities entities = new SalonEntities();
        public MainWindow()
        {
            InitializeComponent();
            foreach(var service in entities.Service)
                listService.Items.Add(service);
        }

        private void listService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_service = listService.SelectedItem as Service;
            if (selected_service != null) 
            {
                tbCost.Text = Convert.ToInt32(selected_service.Cost).ToString();
                tbDescription.Text = selected_service.Description.ToString();
                tbDiscount.Text = selected_service.Discount.ToString();
                tbDuration.Text = selected_service.DurationInSeconds.ToString();
                tbName.Text = selected_service.Title;
            }
            else
            {
                tbDuration.Text = "";
                tbDiscount.Text = "";
                tbDescription.Text = "";
                tbCost.Text = "";
                tbName.Text = "";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var save = listService.SelectedItem as Service;
            if (tbCost.Text == "" || tbDiscount.Text == "" || tbDuration.Text == "" || tbName.Text == "")
                MessageBox.Show("Заполните все поля", "Осечка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if(save == null)
                {
                    save = new Service();
                    entities.Service.Add(save);
                    listService.Items.Add(save);
                }
                save.Cost = Convert.ToDecimal(tbCost.Text);
                save.DurationInSeconds = int.Parse(tbDuration.Text);
                save.Title = tbName.Text;
                save.Description = tbDescription.Text;
                save.Discount = double.Parse(tbDiscount.Text);
                entities.SaveChanges();
                listService.Items.Refresh();
                MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var rezult = MessageBox.Show("Вы точно хотите удалить данные", "Удаление",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rezult == MessageBoxResult.No)
                return;
            var delete = listService.SelectedItem as Service;
            if (delete != null)
            {
                entities.Service.Remove(delete);
                entities.SaveChanges();
                tbCost.Clear();
                tbDescription.Clear();
                tbDiscount.Clear();
                tbDuration.Clear();
                tbName.Clear();
                listService.SelectedIndex = -1;
                listService.Items.Remove(delete);
                MessageBox.Show("Запись успешно удалена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Нет удаляемых записей", "Упс",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbCost.Text = "";
            tbDescription.Text = "";
            tbDiscount.Text = "";
            tbDuration.Text = "";
            tbName.Text = "";
            listService.SelectedIndex = -1;
            tbName.Focus();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            listService.Items.Clear();
            foreach (var country in entities.Service)
                listService.Items.Add(country);

            for (int i = 0; i < listService.Items.Count; i++)
            {
                if (!listService.Items[i].ToString().ToLower().
                    Replace(" ", " ").Contains(tbSearch.Text.ToLower().Replace(" ", " ")))
                {
                    listService.Items.RemoveAt(i--);
                }
            }
        }
    }
}
