using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{

    public static class MP_UIEvents
    {
        public static Action OnContinueActive;
        public static Action OnPauseActive;
    }

    public class MP_UIEventsHandler : Singleton<MP_UIEventsHandler>
    {

        public GameObject Continue;
        public GameObject Pause;

        private void Start()
        {
            MP_UIEvents.OnContinueActive += () => {
                Continue.SetActive(true);
                Pause.SetActive(false);
            };
            MP_UIEvents.OnPauseActive += () => {
                Continue.SetActive(false);
                Pause.SetActive(true);
            };
        }
    }
}

