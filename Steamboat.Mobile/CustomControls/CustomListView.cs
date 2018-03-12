using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class CustomListView : ListView
    {
        #region Properties

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomListView), null, BindingMode.OneWay, propertyChanged: OnItemTappedChanged);

        public static ICommand GetItemTapped(BindableObject bindable)
        {
            return (ICommand)bindable.GetValue(CommandProperty);
        }

        public static void SetItemTapped(BindableObject bindable, ICommand value)
        {
            bindable.SetValue(CommandProperty, value);
        }

        #endregion

        public CustomListView(): base(ListViewCachingStrategy.RecycleElement)
        {

            this.ItemSelected += CustomListView_ItemSelected;

        }

        private void CustomListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            this.SelectedItem = null;
        }

        private static void OnItemTappedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CustomListView;
            if (control != null)
                control.ItemTapped += OnItemTapped;
        }

        private static void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var control = sender as CustomListView;

            var command = GetItemTapped(control);

            if (command != null && command.CanExecute(e.Item))
                command.Execute(e.Item);
        }
    }
}
