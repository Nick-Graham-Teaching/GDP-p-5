using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Game
{
    public class ScreenFrameRate : Singleton<ScreenFrameRate>
    {
        [SerializeField]
        private int _frameRate;

        public int FrameRate
        {
            set
            {
                _frameRate = value;
                Application.targetFrameRate = value;
            }
        }

        private void Start()
        {
            Application.targetFrameRate = _frameRate;
        }
    }
}
