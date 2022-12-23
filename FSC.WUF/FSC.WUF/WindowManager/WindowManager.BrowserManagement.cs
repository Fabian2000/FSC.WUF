using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSC.WUF
{
    public partial class WindowManager
    {
        private Dictionary<int, Action<HtmlDocument>> _events = new Dictionary<int, Action<HtmlDocument>>();

        /// <summary>
        /// This event will call after the whole program loaded
        /// </summary>
        public event EventHandler<EventArgs>? OnLoaded;

        /// <summary>
        /// This event will call after a user clicked on a link
        /// </summary>
        public event EventHandler<WindowManagerLinkEventArgs>? OnPopup;

        /// <summary>
        /// A c# - js bridge for events
        /// </summary>
        /// <param name="element">The element to choose. Similar to querySelectorAll(...)</param>
        /// <param name="eventType">Events are for example click, load, ...</param>
        /// <param name="callback">An action to call after the event happens</param>
        /// <returns></returns>
        public async Task AddEventListener(string element, string eventType, Action<HtmlDocument> callback)
        {
            var id = 0;
            if (_events!.Any())
            {
                id = _events!.Last().Key + 1;
            }

            await ExecuteScript($"addEventListener('{element}', '{eventType}', '{id}');");
            _events!.Add(id, callback);
        }

        /// <summary>
        /// A c# - js bridge for events
        /// </summary>
        /// <param name="element">The element to choose. Similar to querySelectorAll(...)</param>
        /// <param name="eventType">Events are for example click, load, ...</param>
        /// <param name="callback">An action to call after the event happens</param>
        /// <returns></returns>
        public async Task RemoveEventListener(string element, string eventType, Action<HtmlDocument> callback)
        {
            await ExecuteScript($"removeEventListener('{element}', '{eventType}')");
            _events.Remove(
                _events!.Single(e => e.Value == callback).Key
            );
        }
    }
}
