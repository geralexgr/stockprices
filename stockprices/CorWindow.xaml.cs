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
    /// <summary>
    /// Interaction logic for CorWindow.xaml
    /// </summary>
    public partial class CorWindow : Window
    {

        string years = null;
        List<Stock> firStockData = new List<Stock>();
        List<Stock> secStockData = new List<Stock>();
        List<Double> firstReturns = new List<Double>();
        List<Double> secondReturns = new List<Double>();
        double firstAverage = 0, secondAverage = 0;
        double firstVariance, secondVariance;
        double firstMean, secondMean;
        double firstStddev, secStddev;
        public CorWindow()
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

                double result = returnCoefficient(firStockData, secStockData);
                resulttxt.Text = result.ToString() + "% Similarity found!";

            }



        }

        private double returnAverage(List<Stock> l1)
        {
            double average = 0;
            for (int i = 0; i < l1.Count; i++)
            {
                average += l1[i].AdjClose;
            }
            return average;
        }

        private int returnMax(List<Stock> l1, List<Stock> l2)
        {
            int max = 0;
            if (l1.Count > l2.Count)
            {
                max = l2.Count;
            }
            else if (l1.Count < l2.Count)
            {
                max = l1.Count;
            }
            else if ((l1.Count == l2.Count))
            {
                max = l1.Count;
            }
            return max;
        }

        private double returnVariance(List<Stock> l1)
        {
            double variance = 0;
            for (int i = 0; i < l1.Count; i++)
            {
                variance += Math.Pow(l1[i].AdjClose - firstMean, 2);
            }
            return (variance / l1.Count);
        }

        private double returnCov(List<Stock> l1, List<Stock> l2, int size)
        {
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += (l1[i].AdjClose - firstMean) * (l2[i].AdjClose - secondMean);
            }
            return (sum / size);
        }

        private double returnCoefficient(List<Stock> l1, List<Stock> l2)
        {



            firstAverage = returnAverage(l1);
            secondAverage = returnAverage(l2);

            firstMean = firstAverage / (l1.Count - 1);
            secondMean = secondAverage / (l2.Count - 1);

            firstVariance = returnVariance(l1);
            secondVariance = returnVariance(l2);

            firstStddev = Math.Sqrt(firstVariance);
            secStddev = Math.Sqrt(secondVariance);

            double cov = 0;
            int max = returnMax(l1, l2);

            cov = returnCov(l1, l2, max);

            double coefficient = cov / (firstStddev * secStddev);
            coefficient = coefficient * 100;
            return coefficient;
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var value = combo.SelectedIndex;
            years = value.ToString();
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
                return null;
            }

        }







    }
}
