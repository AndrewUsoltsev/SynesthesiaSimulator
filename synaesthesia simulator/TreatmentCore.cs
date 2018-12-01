using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace synaesthesia_simulator
{
    /// <summary>
    /// Модуль, отвечающий за сбор статистики о раскраске букв
    /// </summary>
    public class TreatmentCore 
    {
        public Exercise exercise { get; set; }

        private Collection<Tuple<string, Color>> recordingWords;
        public Stack<string> wordsView { get; private set; }

        public TreatmentCore()
        {
            recordingWords = new Collection<Tuple<string, Color>>();
            wordsView = new Stack<string>();
        }

        public void CreateLesson(Exercise exercise)
        {
            this.exercise = exercise;
            List<string> words = new List<string>();
            try
            {
                words = ReadData();
                // перемешивание
                Random rand = new Random();
                rand.Shuffle<string>(words);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            wordsView.Clear();
            wordsView = new Stack<string>(words);
        }

        public void RecordWord(string word, Color color)
        {
            recordingWords.Add(new Tuple<string, Color>(word,color));
        }

        public void RecordData()
        {
            var pathResult = DataPath.Results(exercise);
            var pathSelection = DataPath.Selection;

            StreamWriter swResult = new StreamWriter(pathResult, append: true);
            StreamWriter swSelection = new StreamWriter(pathSelection, append: true);
            foreach (var elem in recordingWords)
            {
                var tmpColor = elem.Item2;
                swResult.WriteLine(string.Format("{0} {1}", elem.Item1, elem.Item2.ToString()));
                swSelection.WriteLine(string.Format("{0} {1} {2}", tmpColor.R, tmpColor.G, tmpColor.B));
            }
            swResult.Close();
            swSelection.Close();
        }

        private List<string> ReadData()
        {
            List<string> words = new List<string>();
            var path = DataPath.Lesson(exercise);

            if (!File.Exists(path))
            {
                MessageBox.Show("Упражнение не найдено");
                return words; 
            }
            using (StreamReader sr = new StreamReader(path))
            {
                string line = "";
                while (line != null)
                {
                    line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        words.Add(line);
                }

            }
            return words;
        }
      

      
    }
}

public static class RandomExtensions
{
    // Тасование фишера-Йетса
    public static void Shuffle<T>(this Random rng, IList<T> array)
    {
        int n = array.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}