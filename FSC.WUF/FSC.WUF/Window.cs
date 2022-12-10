using FSC.WUF.Enum;
using System;

namespace FSC.WUF
{
    public sealed partial class Window
    {
        public static Window Create(WindowOpenType windowOpenType)
        {
            var windowTemplate = new WindowTemplate();

            switch (windowOpenType)
            {
                case WindowOpenType.Normal: 
                    windowTemplate.Show(); 
                    break;
                case WindowOpenType.Dialog: 
                    windowTemplate.ShowDialog();
                    break;
            }

            return new Window();
        }

        public static Window Create()
        {
            return Create(WindowOpenType.Normal);
        }
    }
}
