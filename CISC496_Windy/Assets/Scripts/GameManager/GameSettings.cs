using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Game
{
    public static class GameSettings 
    {
        private static int _inputDevice = 0;  // 0 == Keyboard; 1 == Mobile

        private static int _frameRate = 1;    // 0 == 30; 1 == 60; 2 == 120

        private static float _sensitivityX = 0; // 0 -> 1
        private static float _sensitivityY = 0; // 0 -> 1


        public static int InputDevice
        {
            get => _inputDevice;
            set
            {
                _inputDevice = value;
                GameEvents.OnInputDeviceChange?.Invoke(value);
            }
        }
        public static int FrameRate
        {
            get => _frameRate;
            set
            {
                _frameRate = value;
                GameEvents.OnFrameRateChange?.Invoke(30 * Mathf.RoundToInt(Mathf.Pow(2, value)));
            }
        }
        public static float SensitivityX
        {
            get => _sensitivityX;
            set
            {
                _sensitivityX = value;
                GameEvents.OnSentivityXChange?.Invoke(value);
            }
        }
        public static float SensitivityY
        {
            get => _sensitivityY;
            set
            {
                _sensitivityY = value;
                GameEvents.OnSentivityYChange?.Invoke(value);
            }
        }
    }
}

