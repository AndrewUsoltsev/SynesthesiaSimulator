using synaesthesia_simulator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace synaesthesia_simulator
{
    public class StatisticExecutor
    {
        private Collection<Color> FavColor { get; set; } 
        private Collection<Tuple<string,Color>> AvgColor { get; set; }
        private string filepath;
        public StatisticExecutor()
        {
            this.filepath = DataPath.StatisticSourse;
            FavColor = new Collection<Color>();
            AvgColor = new Collection<Tuple<string, Color>>();
        }

        public bool ExecuteFavColor()
        {
            FavColor.Clear();
            try
            {
                RunExecuteFavColor();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool ExecuteAvgColor(Exercise exercise)
        {
            var filePath = DataPath.Results(exercise);
            AvgColor.Clear();
            try
            {
                RunExecuteAvgColor(filePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void RunExecuteFavColor()
        {
            ProcessStartInfo start = new ProcessStartInfo()
            {
                FileName = "python",
                Arguments = "\"" + filepath + "\" \"" + DataPath.Selection + "\"",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process process = new Process();
            process = Process.Start(start);
            while (!process.HasExited)
            {
                var readData = process.StandardOutput.ReadLine();
                if (readData != null)
                {
                    string[] array = readData.Split(' ');
                    Color color = Color.FromRgb(
                        Convert.ToByte(array[0]),
                        Convert.ToByte(array[1]),
                        Convert.ToByte(array[2]));
                    FavColor.Add(color);
                }
            }
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new Exception($"Ошибка в исполнении скрипта: {process.ExitCode}");

        }

        private void RunExecuteAvgColor(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception("Файл результатов не найден");

            Collection<Tuple<string, Color>> tmpAvgColor = new Collection<Tuple<string, Color>>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(' ');
                    tmpAvgColor.Add(new Tuple<string, Color>(line[0], (Color)ColorConverter.ConvertFromString(line[1])));
                }
            }
            AveragingColors(tmpAvgColor);
        }

        private void AveragingColors(IEnumerable<Tuple<string, Color>> tmpAvgColor)
        {
            AvgColor.Clear();
            foreach (var colorGroup in tmpAvgColor.GroupBy(x => x.Item1, x => x.Item2))
            {
                int r = 0, g = 0, b = 0;
                int count = 0;
                foreach (var color in colorGroup)
                {
                    r += color.R;
                    g += color.G;
                    b += color.B;
                    count++;
                }
                if (count != 0)
                {
                    r /= count;
                    g /= count;
                    b /= count;
                }
                AvgColor.Add(new Tuple<string, Color>(colorGroup.Key, Color.FromRgb((byte)r, (byte)g, (byte)b)));
            }

        }

        public IEnumerable<FavColor> FavColorResult
        {
            get
            {
                foreach (var elem in FavColor)
                {
                    yield return new FavColor()
                    {
                        ColorName = string.Format("#{0:X2}{1:X2}{2:X2}", elem.R, elem.G, elem.B),
                        ColorView = " ",
                        BackgroundColor = elem.ToString()
                    };
                }
            }
        }

        public IEnumerable<AvgColor> AvgColorResult
        {
            get
            {
                foreach (var elem in AvgColor)
                {
                    yield return new AvgColor()
                    {
                        CharName = elem.Item1,
                        ColorName = string.Format("#{0:X2}{1:X2}{2:X2}", elem.Item2.R, elem.Item2.G, elem.Item2.B),
                        ColorView = " ",
                        BackgroundColor = elem.Item2.ToString()
                    };
                }
            }
        }
    }
}
