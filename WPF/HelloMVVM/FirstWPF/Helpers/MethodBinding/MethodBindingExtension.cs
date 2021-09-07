using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Expression = System.Linq.Expressions.Expression;


namespace FirstWPF.Helpers.MethodBinding
{
    public delegate void MethodProviderHandler(params object[] parameters);

    public class MethodBindingExtension : MarkupExtension
    {
        private class CommandProvider : ICommand
        {
            private readonly MethodBindingProvider _provider;

            public CommandProvider(MethodBindingProvider provider)
            {
                _provider = provider;
            }

            public void Execute(object parameter)
            {
                _provider.MethodProviderHandler(parameter);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }

        private readonly string _method;

        public MethodBindingExtension(string method)
        {
            _method = method;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider is not IProvideValueTarget provideValueTarget || _method == null) return null;

            MethodBindingProvider provider = new MethodBindingProvider(provideValueTarget.TargetObject as FrameworkElement, _method);

            Type memberType = GetMemberType(provideValueTarget.TargetProperty);
            if (memberType == null) throw new InvalidOperationException("Invalid TargetProperty");
            if (typeof(Delegate).IsAssignableFrom(memberType))
            {
                return CreateProxyMethod(memberType, provider.MethodProviderHandler);
            }

            return memberType == typeof(ICommand) ? new CommandProvider(provider) : null;
        }

        private static Delegate CreateProxyMethod(Type handlerType, MethodProviderHandler handler)
        {
            IEnumerable<ParameterInfo> parameterInfos = handlerType.GetMethod("Invoke")?.GetParameters();
            int i = 0;
            var parameters = parameterInfos?.Select(p => Expression.Parameter(p.ParameterType, "x" + (i++))).ToArray();
            var arguments = Expression.NewArrayInit(typeof(object), parameters);
            var body = Expression.Call(Expression.Constant(handler), handler.GetType().GetMethod("Invoke") ?? handler.Method, new Expression[] { arguments });
            var lambda = Expression.Lambda(body, parameters);
            return Delegate.CreateDelegate(handlerType, lambda.Compile(), "Invoke", false);
        }


        private static Type GetMemberType(object targetProperty)
        {
            switch (targetProperty)
            {
                case DependencyProperty property:
                    return property.PropertyType;
                case MemberInfo:
                    switch (targetProperty)
                    {
                        case EventInfo info:
                            return info.EventHandlerType;
                        case PropertyInfo propertyInfo:
                            return propertyInfo.PropertyType;
                    }

                    break;
            }

            return null;
        }
    }
}