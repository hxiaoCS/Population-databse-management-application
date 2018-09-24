using System.Windows;

namespace Population_Database
{
    /// <summary>
    /// Interaction logic for CityDetail.xaml
    /// </summary>
    public partial class CityDetail : Window
    {
        VM vm;
        public CityDetail(VM vm)
        {
            InitializeComponent();
            DataContext = vm; // set up datacontext
            this.vm = vm;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //call save function
            vm.Save();
            //close the window
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //close the window
            this.Close();
        }
    }
}
