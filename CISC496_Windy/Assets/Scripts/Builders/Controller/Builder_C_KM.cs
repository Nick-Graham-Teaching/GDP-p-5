using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{
    public class Builder_C_KM : Controller.Keyboard_Mouse, IBuilder_Controller
    {
        private Controller.Keyboard_Mouse _controller;

        public Builder_C_KM()
        {
            _controller = new Controller.Keyboard_Mouse();
        }

        public Builder_C_KM SetFloatValues(Action<Controller.Keyboard_Mouse> FloatDelegate)
        {
            FloatDelegate?.Invoke(_controller);
            return this;
        }

        public Controller.IController Build() => _controller;
    }
}

