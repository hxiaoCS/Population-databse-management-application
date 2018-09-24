using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Population_Database
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM vm = new VM();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm; // datacontext to the VM
        }

        // create a new item in list
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            vm.EditCity = new CityInfo();
            CityDetail cd = new CityDetail(vm)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            cd.ShowDialog();
        }

        // use edit button or double click to edit the item in list
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedCity != null)
            {
                vm.EditCity = vm.SelectedCity.Copy();
                CityDetail cd = new CityDetail(vm)
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                cd.ShowDialog();
            }
        }

        private void LbData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (vm.SelectedCity != null)
            {
                vm.EditCity = vm.SelectedCity.Copy();
                CityDetail cd = new CityDetail(vm)
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                cd.ShowDialog();
            }
        }

        // delete the item in list
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedCity != null)
                vm.SelectedCity.IsDeleted = true;

            vm.Delete();
        }

        // when the selection in combobox changed, call Load function and change the order
        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.Load();
        }
    }
}