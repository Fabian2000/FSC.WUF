using System;

namespace FSC.WUF
{
    public class WindowManagerLinkEventArgs : EventArgs
    {
        public WindowManagerLinkEventArgs()
        {
        }

        public WindowManagerLinkEventArgs(string link) : base()
        {
            Link = link;
        }

        public string? Link { get; set; }
    }
}
