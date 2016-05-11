using OxyPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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

namespace stockprices
{

    public partial class GraphWindow : Window
    {

        string years = null;
        List<Stock> firStockData = new List<Stock>();
        List<Stock> secStockData = new List<Stock>();

        PlotModel plotModel = new PlotModel();


        public GraphWindow()
        {
            InitializeComponent();
        }


        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {


            if (string.IsNullOrEmpty(firstbox.Text) || string.IsNullOrEmpty(secondbox.Text))
            {
                return;
            }


            DownloadFileStock(firstbox.Text);
            DownloadFileStock(secondbox.Text);

            string firstStock = firstbox.Text + ".csv";
            string secondStock = secondbox.Text + ".csv";


            firStockData = ReadFile(firstStock);
            secStockData = ReadFile(secondStock);

            if (firStockData != null && secStockData != null)
            {
                plotModel.Axes.Add(new LinearAxis { Title = "Days", Position = AxisPosition.Bottom });
                plotModel.Axes.Add(new LinearAxis { Title = "Price", Position = AxisPosition.Left });


                LineSeries fseries = new LineSeries();
                fseries.Color = OxyColors.PaleVioletRed;
                fseries.Title = firstbox.Text + " Stock";

                for (int i = 0; i < firStockData.Count; i++)
                {
                    fseries.Points.Add(new OxyPlot.DataPoint(i, firStockData[i].Close));
                }

                LineSeries secseries = new LineSeries();
                secseries.Color = OxyColors.LimeGreen;
                secseries.Title = secondbox.Text + " Stock";

                for (int i = 0; i < secStockData.Count; i++)
                {
                    secseries.Points.Add(new OxyPlot.DataPoint(i, secStockData[i].Close));
                }


                plotModel.Series.Add(fseries);
                plotModel.Series.Add(secseries);
                plot.Model = plotModel;
            }



        }

        private void DownloadFileStock(string name)
        {
            try
            {


                string fileName = name + ".csv";
                WebClient client = new WebClient();
                if (years == null)
                {
                    client.DownloadFile(new Uri("http://real-chart.finance.yahoo.com/table.csv?s=" + name + "&a=00&b=1&c=1996&d=00&e=1&f=2016&g=d&ignore=.csv"), fileName);
                }
                else if (years == "0")
                {
                    client.DownloadFile(new Uri("http://real-chart.finance.yahoo.com/table.csv?s=" + name + "&a=00&b=1&c=2011&d=00&e=1&f=2016&g=d&ignore=.csv"), fileName);
                }

                else if (years == "1")
                {
                    client.DownloadFile(new Uri("http://real-chart.finance.yahoo.com/table.csv?s=" + name + "&a=00&b=1&c=1996&d=00&e=1&f=2016&g=d&ignore=.csv"), fileName);
                }
                else if (years == "2")
                {
                    client.DownloadFile(new Uri("http://real-chart.finance.yahoo.com/table.csv?s=" + name + "&a=00&b=1&c=1986&d=00&e=1&f=2016&g=d&ignore=.csv"), fileName);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("An error occured. Make sure stock ticker is correct");
            }
        }




        private static List<Stock> ReadFile(string filepath)
        {
            try
            {


                var lines = File.ReadAllLines(filepath);

                var data = from l in lines.Skip(1)
                           let split = l.Split(',')
                           select new Stock
                           {
                               Date = split[0],
                               Open = Double.Parse(split[1], CultureInfo.InvariantCulture),
                               High = Double.Parse(split[2], CultureInfo.InvariantCulture),
                               Low = Double.Parse(split[3], CultureInfo.InvariantCulture),
                               Close = Double.Parse(split[4], CultureInfo.InvariantCulture),
                               Volume = Double.Parse(split[5], CultureInfo.InvariantCulture),
                               AdjClose = Double.Parse(split[6], CultureInfo.InvariantCulture)
                           };

                return data.ToList();

            }
            catch (Exception)
            {
                //MessageBox.Show("An error occured!");
                return null;
            }

        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var value = combo.SelectedIndex;
            years = value.ToString();
        }





    }
}
