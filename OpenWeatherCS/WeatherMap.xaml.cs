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
using System.Windows.Shapes;


namespace OpenWeatherCS
{
    /// <summary>
    /// Interaction logic for WeaterMap.xaml
    /// </summary>
    public partial class WeaterMap : Window
    {
        public WeaterMap()
        {
            InitializeComponent();
        }

        private void ButtonHaifa_Click(object sender, RoutedEventArgs e)
        {
            
            
            MainWindow mw = new MainWindow();
            // mw.ShowDialog();
            mw.LocationTextBox.Text = "Haifa";
           //w.LocationTextBox.Text = Key.Enter;
            mw.ShowDialog();
           // SendKeys.Send("{ENTER}");


        }

        private void ButtonTelAviv_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.LocationTextBox.Text = "Tel Aviv";
            mw.ShowDialog();
        }

        private void ButtonBeerSheva_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.LocationTextBox.Text = "Beer Sheva";
            mw.ShowDialog();
        }

        private void ButtonJerusalem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.LocationTextBox.Text = "Jerusalem";
            mw.ShowDialog();
        }

        private void ButtonEilat_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.LocationTextBox.Text = "Eilat";
            mw.ShowDialog();
        }
    }
}
