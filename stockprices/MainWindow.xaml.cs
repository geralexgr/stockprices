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

namespace stockprices
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void graphBtn_Click(object sender, RoutedEventArgs e)
        {


            GraphWindow window = new GraphWindow();
            window.Show();

        }

        private void algoBtn_Click(object sender, RoutedEventArgs e)
        {
            CorWindow window = new CorWindow();
            window.Show();
        }




    }
}
