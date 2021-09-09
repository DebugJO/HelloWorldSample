using System.Linq;
using System.Windows;

namespace FirstWPF.Helpers.SystemHelper
{
    public static class IoC
    {
        /// <summary>
        /// Ioc View Only
        /// </summary>
        /// <typeparam name="T">View</typeparam>
        /// <returns>Veiw Instance</returns>
        public static T Get<T>() where T : Window
        {
            return Application.Current.Windows.OfType<T>().First();
        }

        /// <summary>
        /// IoC ViewModel Only
        /// </summary>
        /// <typeparam name="T">View</typeparam>
        /// <typeparam name="TU">ViewModel</typeparam>
        /// <returns>ViewModel Instance</returns>
        public static TU Get<T, TU>() where T : Window where TU : class
        {
            return (TU)(Get<T>().DataContext);
        }
    }
}