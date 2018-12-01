using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synaesthesia_simulator
{
    public static class DataPath
    {
        public static string GetExerciseFileName(Exercise exercise)
        {
            string fileName = "";
            switch (exercise)
            {
                case Exercise.Begin:
                    fileName = "Begin.txt"; break;
                case Exercise.Standart:
                    fileName = "Standart.txt"; break;
                case Exercise.Additional:
                    fileName = "Additional.txt"; break;
                default:
                    fileName = null; break;
            }
            return fileName;
        }

        public static string Lesson(Exercise exercise)
        {
            if ((int)exercise == -1)
                return null;
            string fileName = GetExerciseFileName(exercise);

            var curDir = Directory.GetCurrentDirectory();
            var GeneralDir = Directory.GetParent(curDir).Parent.FullName; 

            return Path.Combine(GeneralDir, "Lessons", fileName);
        }

        public static string Results(Exercise exercise)
        {
            if ((int)exercise == -1)
                return null;
            string fileName = GetExerciseFileName(exercise);

            var curDir = Directory.GetCurrentDirectory();
            var GeneralDir = Directory.GetParent(curDir).Parent.FullName; 

            return Path.Combine(GeneralDir, "Result", fileName);
        }

        public static string Selection
        {
            get
            {
                var curDir = Directory.GetCurrentDirectory();
                var GeneralDir = Directory.GetParent(curDir).Parent.FullName; 

                return Path.Combine(GeneralDir, "Result", "SelectionRGB.txt");
            }
        }

        public static string StatisticSourse
        {
            get
            {
                var curDir = Directory.GetCurrentDirectory();
                var GeneralDir = Directory.GetParent(curDir).Parent.FullName; 

                return Path.Combine(GeneralDir, "StatisticSource.py");
            }
        }

    }
}
