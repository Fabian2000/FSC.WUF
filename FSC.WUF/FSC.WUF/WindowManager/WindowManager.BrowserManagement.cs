using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSC.WUF
{
    public partial class WindowManager
    {
        private Dictionary<int, Action> _events = new Dictionary<int, Action>();

        public event EventHandler<EventArgs> OnLoaded;

        public event EventHandler<WindowManagerLinkEventArgs> OnPopup;

        public async Task AddEventListener(string element, string eventType, Action callback)
        {
            var id = 0;
            if (_events!.Any())
            {
                id = _events!.Last().Key + 1;
            }

            await ExecuteScript($"addEventListener('{element}', '{eventType}', '{id}');");
            _events!.Add(id, callback);
        }

        public async Task RemoveEventListener(string element, string eventType, Action callback)
        {
            await ExecuteScript($"removeEventListener('{element}', '{eventType}')");
            _events.Remove(
                _events!.Single(e => e.Value == callback).Key
            );
        }
    }
}
