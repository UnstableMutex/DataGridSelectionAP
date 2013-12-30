using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using DataGridRow = System.Windows.Controls.DataGridRow;

namespace TaskDistribution.Test
{
    public class DataGridSelectionAP
    {
        public static readonly DependencyProperty CustomSelectionProperty = DependencyProperty.RegisterAttached(
  "CustomSelection",
  typeof(Boolean),
  typeof(DataGrid),
  new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnPch)
);

        static void OnPch(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg == null)
                return;
            if (e.NewValue.GetType() != typeof(bool))
            {
                throw new NotSupportedException();
            }

            var nv = (bool)e.NewValue;

            if (nv)
            {
                dg.PreviewMouseLeftButtonDown += UIElement_OnPreviewMouseLeftButtonDown;
            }

        }
        public static void SetCustomSelection(UIElement element, Boolean value)
        {
            element.SetValue(CustomSelectionProperty, value);
        }
        public static Boolean GetCustomSelection(UIElement element)
        {
            return (Boolean)element.GetValue(CustomSelectionProperty);
        }
        static DataGridRow GetRow(DataGrid dg, int index)
        {
            if (dg.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return null;
            }
            else
            {
                return dg.ItemContainerGenerator.ContainerFromIndex(index) as System.Windows.Controls.DataGridRow;
            }

        }
        static DataGridRow GetRowBelowMouse(DataGrid dg, MouseButtonEventArgs e)
        {

            for (int i = 0; i < dg.Items.Count; i++)
            {
                DataGridRow r = GetRow(dg, i);
                Rect posBounds = VisualTreeHelper.GetDescendantBounds(r);
                Point pos = e.GetPosition(r);
                if (posBounds.Contains(pos))
                {
                    return r;
                }
            }
            return null;

        }
        private static void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;
            var r = GetRowBelowMouse(dg, e);
            if (r.IsSelected)
            {
                e.Handled = true;
            }
        }
    }
}
