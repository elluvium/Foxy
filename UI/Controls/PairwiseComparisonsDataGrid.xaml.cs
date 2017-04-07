using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.Controls;

namespace UI.Controls
{



    /// <summary>
    /// Interaction logic for PairwiseComparisonsDataGrid.xaml
    /// </summary>
    public partial class PairwiseComparisonsDataGrid : DataGrid
    {
        private Data.Matrixes.PairwiseComparisonsMatrix<Data.BusinessStructures.Aspect> _matrix;
        

        public PairwiseComparisonsDataGrid(Data.Matrixes.PairwiseComparisonsMatrix<Data.BusinessStructures.Aspect> matrix)
        {
            InitializeComponent();
            _matrix = matrix;
            DataContext = _matrix;
            CellEditEnding += DataGrid_CellEditEnding;
        }


        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            int row = Items.IndexOf(e.Row.Item);
            int column = Columns.ToList().IndexOf(e.Column);
            if (row != column)
            {
                int selectedValue;
                if (int.TryParse(((TextBox)e.EditingElement).Text, out selectedValue))
                {
                    if (Data.Matrixes.PairwiseComparisonsMatrix<Data.BusinessStructures.Aspect>.CheckValidity(selectedValue))
                    {
                        _matrix.SetPreference(row, column, selectedValue);
                        GetCell(this, column, row).Content = _matrix[column, row];
                        e.Cancel = false;
                        return;
                    }
                }
            }
            ((TextBox)e.EditingElement).Text = _matrix[row, column].ToString();
            e.Cancel = true;
        }

        /// <summary>
        ///  Next part of code (crutch to be honest) is stolen from http://stackoverflow.com/questions/12164079/change-datagrid-cell-value-programmatically-in-wpf#12164533
        ///  
        ///  Don't blame me, it's already 3am and I want to sleep.
        /// </summary>

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static DataGridRow GetSelectedRow(DataGrid grid)
        {
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }
        public static DataGridRow GetRow(DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public static DataGridCell GetCell(DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public static DataGridCell GetCell(DataGrid grid, int row, int column)
        {
            DataGridRow rowContainer = GetRow(grid, row);
            return GetCell(grid, rowContainer, column);
        }
    }
}
