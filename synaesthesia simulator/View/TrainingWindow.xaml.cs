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
using Xceed.Wpf.Toolkit;

namespace synaesthesia_simulator
{
    /// <summary>
    /// Логика взаимодействия для TrainingWindow.xaml
    /// </summary>
    public partial class TrainingWindow : Window
    {
        Exercise exercise;
        TreatmentCore treatmentCore;
        int countLesson = 0;
        int countDone = 0;
        string currentWord = "";
        public TrainingWindow(Exercise exercise)
        {
            InitializeComponent();
            treatmentCore = new TreatmentCore();
            this.exercise = exercise;
            try
            {
                treatmentCore.CreateLesson(exercise);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                this.Close();
            }
            countLesson = treatmentCore.wordsView.Count;
            CountLabel.Content = "Выполнено " + countDone.ToString() + " из " + countLesson.ToString(); // переписать в функцию
            currentWord = treatmentCore.wordsView.Pop();
            WordLabel.Content = currentWord;

            colorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
            this.Closed += TrainingWindow_Closed;
        }


        private void TrainingWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                treatmentCore.RecordData();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }

            if (this.Owner != null)
                this.Owner.Visibility = Visibility.Visible;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            WordLabel.Foreground = new SolidColorBrush((e.Source as ColorPicker).SelectedColor ?? Brushes.Black.Color);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            countDone++;
            CountLabel.Content = string.Format(" Выполнено {0} из {1}", countDone.ToString(), countLesson.ToString());
            Color? color = colorPicker.SelectedColor;
            treatmentCore.RecordWord(currentWord, color ?? new Color() );
            if (countDone != countLesson)
            {
                currentWord = treatmentCore.wordsView.Pop();
                WordLabel.Content = currentWord;
            }
            else
            {
                WordLabel.Content = "Урок закончен!";
                NextButton.Visibility = Visibility.Hidden;
                colorPicker.Visibility = Visibility.Hidden;
            }
            colorPicker.SelectedColor = Brushes.Black.Color;
        }

    }
}
