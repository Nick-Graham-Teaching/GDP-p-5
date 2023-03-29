using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Controller
{
    public interface IController : IBehaviour
    {

        public abstract float GetCameraMoveAxisX();
        public abstract float GetCameraMoveAxisY();

        public abstract bool GetKeyDown(KeyCode key, out float degree);
        public abstract bool GetKeyWait(KeyCode key, out float degree);
        public abstract bool GetKeyPress(KeyCode key, out float degree);
        public abstract bool GetKeyTap(KeyCode key, out float degree);
        public abstract bool GetKeyUp(KeyCode key, out float degree);
        public abstract bool GetKeyIdle(KeyCode key, out float degree);

        public abstract bool GetAllDirectionKeysIdle();
    }
}

