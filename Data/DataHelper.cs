using Excel.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
    using BusinessStructures;

    public static class DataHelper
    {
        public static List<Goal> ReadFromXLSX(string fullPath)
        {
            List<Goal> result = new List<Goal>();

            using (var stream = File.OpenRead(fullPath))
            {
                using (ExcelDataReaderHelper excelHelper = new ExcelDataReaderHelper(stream))
                {
                    result = excelHelper.GetRange<Goal>(0, 1, 1).ToList();
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

    }

    public static class MathHelper
    {
        public static bool EqualsWithError(this double thisVariable, double variable,  double error = 0.000001)
        {
            return Math.Abs(thisVariable - variable) < error;
        }
    }
}
