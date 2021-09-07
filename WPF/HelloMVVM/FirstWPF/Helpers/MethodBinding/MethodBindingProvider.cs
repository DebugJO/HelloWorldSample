using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace FirstWPF.Helpers.MethodBinding
{
    internal class MethodBindingProvider : DependencyObject
    {
        private MethodInfo _methodInfo;
        private ParameterInfo[] _parameters;
        private readonly string _methodName;

        public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), typeof(MethodBindingProvider), new UIPropertyMetadata(null));

        public MethodBindingProvider(FrameworkElement element, string methodPath)
        {
            string propertyPath = string.Empty;
            int index = methodPath.LastIndexOf('.');
            if (index > 0)
            {
                propertyPath = "." + methodPath[..index];
                methodPath = methodPath[(index + 1)..];
            }

            _methodName = methodPath;
            BindingOperations.SetBinding(this, DataContextProperty, new Binding { Source = element, Path = new PropertyPath("DataContext" + propertyPath) });
            OnInitializeMethodProxy();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == DataContextProperty) OnInitializeMethodProxy();
        }


        private void OnInitializeMethodProxy()
        {
            _methodInfo = GetValue(DataContextProperty) == null ? null : this.GetValue(DataContextProperty).GetType().GetMethod(_methodName);
            _parameters = _methodInfo != null ? _methodInfo.GetParameters() : null;
        }


        internal void MethodProviderHandler(params object[] parameters)
        {
            if (_methodInfo == null || GetValue(DataContextProperty) == null || _parameters == null) return;
            _methodInfo.Invoke(GetValue(DataContextProperty), parameters.Take(_parameters.Length).ToArray());
        }
    }
}