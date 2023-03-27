using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Windy.UI
{
    public class PopupWindow
    {
        public UnityEngine.UI.Image Window;
        public PopupWindow() { }
        public PopupWindow(UnityEngine.UI.Image image)
        {
            Window = image;
        }

        public virtual bool Available => false;
    }
    public class VoidWindow : PopupWindow
    {
        public override bool Available => true;
    }
    public class MCConnecionWindow : PopupWindow
    {
        public MCConnecionWindow(UnityEngine.UI.Image image) : base(image) { }
    }
    public class MCDisconnectionWindow : PopupWindow
    {
        public MCDisconnectionWindow(UnityEngine.UI.Image image) : base(image) { }
    }
}