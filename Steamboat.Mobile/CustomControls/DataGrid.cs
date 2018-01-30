using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Steamboat.Mobile.Models.Participant;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class DataGrid : Grid
    {
        #region Properties

        public static readonly BindableProperty DataSourceProperty =
            BindableProperty.Create(nameof(DataSource), typeof(IEnumerable), typeof(DataGrid), default(IEnumerable<object>), BindingMode.TwoWay, propertyChanged: OnDataSourcePropertyChanged);

        public static readonly BindableProperty DataTemplateProperty =
            BindableProperty.Create(nameof(DataTemplate), typeof(DataTemplate), typeof(DataGrid), default(DataTemplate));

        public static readonly BindableProperty ColumnsProperty =
            BindableProperty.Create(nameof(Columns), typeof(int), typeof(DataGrid), default(int));

        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(DataGrid), null);

        public static readonly BindableProperty SeparatorColorProperty =
            BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(DataGrid), Color.FromHex("#FFFFFF"));

        public IEnumerable DataSource
        {
            get { return (IEnumerable)this.GetValue(DataSourceProperty); }
            set { this.SetValue(DataSourceProperty, value); }
        }

        public DataTemplate DataTemplate
        {
            get { return (DataTemplate)GetValue(DataTemplateProperty); }
            set { SetValue(DataTemplateProperty, value); }
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        #endregion

        public DataGrid()
        {
            for (int i = 0; i < Columns; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
        }

        private static void OnDataSourcePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var itemsLayout = (DataGrid)bindable;
            itemsLayout.SetItems();
        }

        protected virtual void SetItems()
        {
            this.Children.Clear();

            if (DataSource == null)
                return;

            var row = 0;
            var col = 0;

            var dataMatrix = DataSource.Cast<IEnumerable>();
            foreach (var containerList in dataMatrix)
            {
                var lastItemCount = (containerList as ICollection).Count;
                AddElementToRow(ref row, ref col, containerList, lastItemCount);

                if (ThereIsAnotherGroup(dataMatrix, containerList))
                {
                    var boxView = DrawSeparator(row);
                    row++;
                    SetColumnSpan(boxView, Columns);
                }
            }
        }

        private void AddElementToRow(ref int row, ref int col, IEnumerable containerList, int lastItemCount)
        {
            var itemCount = 0;
            foreach (var item in containerList)
            {
                Children.Add(GetItemView(item), col, row);
                col++;
                itemCount++;
                if (ShouldBreakLine(col, lastItemCount, itemCount))
                {
                    row++;
                    col = 0;
                    this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                }
            }
        }

        private bool ThereIsAnotherGroup(IEnumerable<IEnumerable> dataMatrix, IEnumerable containerList)
        {
            return !dataMatrix.Last().Equals(containerList);
        }

        private BoxView DrawSeparator(int row)
        {
            var boxView = new BoxView();
            boxView.HeightRequest = 1;
            boxView.Margin = new Thickness(0, 10);
            boxView.Color = SeparatorColor;
            Children.Add(boxView, 0, row);
            return boxView;
        }

        private bool ShouldBreakLine(int col, int lastItemCount, int itemCount)
        {
            return col.Equals(Columns) || itemCount.Equals(lastItemCount);
        }

        protected virtual View GetItemView(object item)
        {
            var content = DataTemplate.CreateContent();
            var view = content as Button;
            if (view == null)
            {
                return null;
            }

            view.BindingContext = item;
            view.Command = SelectedCommand;
            view.CommandParameter = item;

            return view;
        }
    }
}
