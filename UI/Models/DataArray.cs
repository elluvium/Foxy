using System;
using System.ComponentModel;
using System.Data;

namespace UI.Models
{
    static class TConverter
    {
        public static T ChangeType<T>(object value)
        {
            return (T)ChangeType(typeof(T), value);
        }
        public static object ChangeType(Type t, object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }
        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {

            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }

    /// <summary>
    /// Класс-прослойка
    /// для работы с DataGrid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataArray<T>
    {

        public class DataArrayRow
        {
            DataTable data;
            int row;

            public DataArrayRow(DataTable data, int row)
            {
                this.row = row;
                this.data = data;
            }

            public T this[int index]
            {
                get { return TConverter.ChangeType<T>(data.Rows[row][index] + ""); }
                set { data.Rows[row].SetField<T>(index, value); }
            }
        }

        DataTable data = new DataTable();
        int rows, columns;

        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }

        public DataTable Data
        {
            get { return data; }
        }

        public DataArrayRow this[int index]
        {
            get { return new DataArrayRow(data, index); }
        }

        public DataArray(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;

            for (int j = 0; j < columns; j++)
                data.Columns.Add((j + 1) + "");

            for (int i = 0; i < rows; i++)
                data.Rows.Add();
        }

    }
}
