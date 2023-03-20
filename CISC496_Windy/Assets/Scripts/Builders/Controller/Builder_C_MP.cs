using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{
    public class Builder_C_MP : Controller.MobilePhoneServer, IBuilder_Controller
    {
        private Controller.MobilePhoneServer _controller;

        public Builder_C_MP()
        {
            _controller = new Controller.MobilePhoneServer();
        }

        public new void InitNetwork()
        {
            _controller.InitNetwork();
        }

        public new void ResetGyroAxes()
        {
            _controller.ResetGyroAxes();
        }

        public new void SetPhoneContinueActive(bool active)
        {
            _controller.SetPhoneContinueActive(active);
        }

        [System.Obsolete]
        public new void SetUseGyroActive(bool flag)
        {
            _controller.SetUseGyroActive(flag);
        }


        public Controller.IController Build() => _controller;
    }
}

