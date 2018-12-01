using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using synaesthesia_simulator.Model;

namespace synaesthesia_simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IEnumerable<FavColor> favColorItems { get; set; }
        public IEnumerable<AvgColor> avgColorItems { get; set; }
        private StatisticExecutor statisticExecutor;
        private int prevSelectIndexAvgColor {get;set;}
        public MainWindow()
        {
            InitializeComponent();
            statisticExecutor = new StatisticExecutor();
            prevSelectIndexAvgColor = ExerciseChoiseAvgColorsComboBox.SelectedIndex;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new TrainingWindow((Exercise)ExerciseChoiseComboBox.SelectedIndex);
            form.Owner = this;

            this.Visibility = Visibility.Hidden;
            try
            {
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (statisticExecutor.ExecuteFavColor()) 
            {
                favColorItems = statisticExecutor.FavColorResult; 
                FavoriteColorsListView.ItemsSource = favColorItems; 
            }
            if (ExerciseChoiseAvgColorsComboBox.SelectedIndex != -1 
                && statisticExecutor.ExecuteAvgColor((Exercise)ExerciseChoiseAvgColorsComboBox.SelectedIndex))
            {
                avgColorItems = statisticExecutor.AvgColorResult;
                AverageColorsListBox.ItemsSource = avgColorItems; 
            }
            AverageColorsListBox.UpdateLayout();
            FavoriteColorsListView.UpdateLayout();
        }


        private void ExerciseChoiseAvgColorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (statisticExecutor.ExecuteAvgColor((Exercise)ExerciseChoiseAvgColorsComboBox.SelectedIndex))
            {
                avgColorItems = statisticExecutor.AvgColorResult;
                AverageColorsListBox.ItemsSource = avgColorItems; 
                prevSelectIndexAvgColor = (sender as ComboBox).SelectedIndex;
                AverageColorsListBox.UpdateLayout();
            }
            else
            {
                ExerciseChoiseAvgColorsComboBox.SelectedIndex = prevSelectIndexAvgColor;
            }
        }
    }
}
