using System.Threading.Tasks;

namespace FSC.WUF
{
    /// <summary>
    /// A C# - JavaScript bridge for the navigator
    /// </summary>
    public class Navigator
    {
        private readonly WindowManager _window;

        internal Navigator(WindowManager window)
        {
            _window = window;
        }

        /// <summary>
        /// Gets the online state of the client
        /// </summary>
        /// <returns>Returns if the client is online or offline</returns>
        public async Task<bool> OnLine()
        {
            var online = await _window.ExecuteScript("navigator.onLine");

            if (online.Contains("true", System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class NavigatorEx
    {
        /// <summary>
        /// Gets a new navigator
        /// </summary>
        /// <returns>Returns a new navigator</returns>
        public static Navigator GetNavigator(this WindowManager window)
        {
            return new Navigator(window);
        }
    }
}
