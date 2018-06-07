using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class ModalViewModelAutoWire
    {
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelAutoWire), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelAutoWire.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelAutoWire.AutoWireViewModelProperty, value);
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var modalView = bindable as Element;
            if (modalView == null)
            {
                return;
            }

            var modalViewType = modalView.GetType();
            var modalViewName = modalViewType.FullName.Replace(".Views.Modals", ".ViewModels.Modals");
            var modalViewAssemblyName = modalViewType.GetTypeInfo().Assembly.FullName;
            var modalViewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", modalViewName, modalViewAssemblyName);

            var modalViewModelType = Type.GetType(modalViewModelName);
            if (modalViewModelType == null)
            {
                return;
            }
            var viewModel = DependencyContainer.Resolve(modalViewModelType);
            modalView.BindingContext = viewModel;
        }

    }
}
