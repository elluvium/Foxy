using Excel.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Data
{
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
    }
}
