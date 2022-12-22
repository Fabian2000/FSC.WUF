using System;

namespace FSC.WUF
{
    /// <summary>
    /// Contains an extra property to get the requested popup url
    /// </summary>
    public class WindowManagerLinkEventArgs : EventArgs
    {
    /// <summary>
    /// Contains an extra property to get the requested popup url
    /// </summary>
        public WindowManagerLinkEventArgs()
        {
        }

        /// <summary>
        /// Contains an extra property to get the requested popup url
        /// </summary>
        /// <param name="link">Attribute to set the link</param>
        public WindowManagerLinkEventArgs(string link) : base()
        {
            Link = link;
        }

        /// <summary>
        /// The link (url) from the popup event
        /// </summary>
        public string? Link { get; set; }
    }
}
