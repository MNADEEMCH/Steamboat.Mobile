using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Steamboat.Mobile.CustomControls
{
    public class RepeaterControl : StackLayout
    {
        #region Properties

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(RepeaterControl), default(IEnumerable<object>), BindingMode.OneWay, propertyChanged: OnItemsSourcePropertyChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterControl), default(DataTemplate), propertyChanged: OnItemTemplatePropertyChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public delegate void RepeaterViewItemAddedEventHandler(object sender, RepeaterControlItemAddedEventArgs args);
        public event RepeaterViewItemAddedEventHandler ItemCreated;

        protected virtual void NotifyItemAdded(View view, object model)
        {
            ItemCreated?.Invoke(this, new RepeaterControlItemAddedEventArgs(view, model));
        }

        #endregion

        public RepeaterControl()
        {
            Spacing = 0;
        }

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RepeaterControl)bindable;

            var oldObservableCollection = oldValue as INotifyCollectionChanged;

            if (oldObservableCollection != null)
            {
                oldObservableCollection.CollectionChanged -= control.OnItemsSourceCollectionChanged;
            }

            var newObservableCollection = newValue as INotifyCollectionChanged;

            if (newObservableCollection != null)
            {
                newObservableCollection.CollectionChanged += control.OnItemsSourceCollectionChanged;
            }

            if (newValue == null)
            {
                return;
            }

            control.PopulateItems();
        }

        private static void OnItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RepeaterControl)bindable;
            if (newValue == null)
            {
                return;
            }

            control.PopulateItems();
        }

        private void PopulateItems()
        {
            var items = ItemsSource;
            if (items == null || ItemTemplate == null)
            {
                return;
            }

            var children = Children;
            children.Clear();

            foreach (var item in items)
            {
                children.Add(InflateView(item));
            }
        }

        private View InflateView(object viewModel)
        {
            var view = (View)CreateContent(ItemTemplate, viewModel, this);
            view.BindingContext = viewModel;
            return view;
        }

        private static object CreateContent(DataTemplate template, object item, BindableObject container)
        {
            var selector = template as DataTemplateSelector;
            if (selector != null)
            {
                template = selector.SelectTemplate(item, container);
            }

            return template.CreateContent();
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var children = Children;
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var view = InflateView(item);
                    children.Add(view);
                    NotifyItemAdded(view, item);
                    ScrollToBottom(view);
                }
            }
        }

        private void ScrollToBottom(View view)
        {
            var repeater = view.Parent as RepeaterControl;
            var scroll = repeater?.Parent as ScrollView;
            if (scroll != null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var currentHeight = scroll.Height;
                    var height = scroll.Content.Height;
                    var paddingTop = scroll.Padding.Top;
                    var paddingBottom = scroll.Padding.Bottom;

                    if (height > currentHeight)
                        await scroll.ScrollToAsync(0, height+paddingTop+paddingBottom - currentHeight + 10, true);
                    else
                        await Task.FromResult(true);
                });
            }
        }
    }

    public class RepeaterControlItemAddedEventArgs : EventArgs
    {
        public RepeaterControlItemAddedEventArgs(View view, object model)
        {
            View = view;
            Model = model;
        }

        public View View { get; set; }
        public object Model { get; set; }
    }
}
