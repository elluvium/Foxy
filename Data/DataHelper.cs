using Excel.Helper;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
    using BusinessStructures;

    public class GoalExcel : Goal
    {
        private string _providesFor = string.Empty;

        public string ProvidesFor
        {
            get
            {
                return _providesFor;
            }
            set
            {
                if(value != null)
                {
                    _providesFor = value;
                }
            }
        }


        public uint[] GetIndexesFromProvidesFor()
        {
            var stringIndexes = ProvidesFor.Split(',', ' ');
            try
            {
                return stringIndexes.Where(index => index != "").Select(index => uint.Parse(index)).ToArray();
            }
            catch
            {
                throw new InvalidCastException("Wrong format of ProvidesFor field");
            }
        }
    }

    public static class DataHelper
    {
        public static IEnumerable<GoalExcel> ReadFromXLSX(string fullPath)
        {
            List<GoalExcel> result = new List<GoalExcel>();

            using (var stream = File.OpenRead(fullPath))
            {
                using (ExcelDataReaderHelper excelHelper = new ExcelDataReaderHelper(stream))
                {
                    result = excelHelper.GetRange<GoalExcel>(0, 1, 1).ToList();
                }
            }

            return result;
        }

        public static void SerializeBS(BusinessSystem bs, string fullPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, bs);
            }
        }

        public static BusinessSystem DeserializeBS(string fullPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            BusinessSystem deserializedBS = new BusinessSystem();

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                deserializedBS = (BusinessSystem)formatter.Deserialize(fs);
            }

            return deserializedBS;
        }


        public static Matrixes.IncidenceMatrix<Goal> Convert(IEnumerable<GoalExcel> goals)
        {
            var matrix = new Matrixes.IncidenceMatrix<Goal>(goals);
            foreach(var goalExcel in goals)
            {
                var indexes = goalExcel.GetIndexesFromProvidesFor();
                foreach(var index in indexes)
                {
                    var matchGoalExcel = goals.FirstOrDefault(x => x.Index == index);
                    if(matchGoalExcel == null)
                    {
                        throw new InvalidCastException("Wrong format of ProvidesFor field: Undescribed indexes detected.");
                    }
                    matrix[goalExcel, matchGoalExcel] = true;
                }
            }
            return matrix;
        }
    }

    public static class MathHelper
    {
        public static bool EqualsWithError(this double thisVariable, double variable,  double error = 0.000001)
        {
            return Math.Abs(thisVariable - variable) < error;
        }

       
    }
}
