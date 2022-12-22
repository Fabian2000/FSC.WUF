using System.Threading.Tasks;

namespace FSC.WUF
{
    public class Navigator
    {
        private readonly WindowManager _window;

        internal Navigator(WindowManager window)
        {
            _window = window;
        }

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

    public static class NavigatorEx
    {
        public static Navigator GetNavigator(this WindowManager window)
        {
            return new Navigator(window);
        }
    }
}
