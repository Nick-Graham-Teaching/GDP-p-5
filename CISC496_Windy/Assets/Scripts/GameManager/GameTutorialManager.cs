using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Game
{
    public class GameTutorialManager : StaticSingleton<GameTutorialManager>
    {
        [SerializeField] UnityEngine.UI.Image PointerEventImage;

        static int _flyTutorialCount = 0;
        static int _glidePunishmentTutorialCount = 0;
        static int _puzzleHintTutorialCount = 0;
        static int _mobileControlTutorialCount = 0;
        static int _waystoneTutorialCount = 0;

        public static bool IsDisplayAll { get; private set; }

        private void Update()
        {
            if (GameProgressManager.Instance.GameState.IsInGame())
            {
                if (Input.GetKeyDown(Keys.TutorialCode))
                {
                    DisplayAll();
                }
            }
        }

        public static void ResetTutorialCount()
        {
            _flyTutorialCount = 0;
            _glidePunishmentTutorialCount = 0;
            _puzzleHintTutorialCount = 0;
            _mobileControlTutorialCount = 0;
            _waystoneTutorialCount = 0;
        }

        public static void DisplayAll()
        {
            if (!IsDisplayAll)
            {
                IsDisplayAll = true;
                Pause();
                UI.UI_GameMessage.DisplayAll();
            }
        }

        public static void TurnOffTut()
        {
            if (IsDisplayAll)
            {
                IsDisplayAll = UI.UI_GameMessage.DisplayAll();
                if (!IsDisplayAll)
                {
                    TurnOffTut();
                }
                return;
            }

            GameProgressManager.Instance.GameState = new Continue();
            Instance.StopAllCoroutines();
            Instance.PointerEventImage.enabled = false;
            UI.UI_GameMessage.TurnOffAllMessages();
        }

        IEnumerator ContinueKeySupervisor()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(Keys.ContinueCode) || Controller.Controller.ControlDevice.GetKeyDown(Keys.ContinueCode, out float _));
            TurnOffTut();
            if (IsDisplayAll)
            {
                yield return null;
                StartCoroutine(ContinueKeySupervisor());
            }
        }
        static void Pause()
        {
            GameProgressManager.Instance.GameState = new Pause();
            UI.UIEventsHandler.Instance.PausePage.SetActive(false);
            Instance.PointerEventImage.enabled = true;
            Instance.StartCoroutine(Instance.ContinueKeySupervisor());
        }

        public static void DisplayMobilePhoneControl()
        {
            if (_mobileControlTutorialCount++ != 0) return;
            Pause();
            UI.UI_GameMessage.DisplayMobileControllerTutorialMessage();
        }
        public static void DisplayFlyTutorial()
        {
            if (_flyTutorialCount++ != 0) return;
            Pause();
            UI.UI_GameMessage.DisplayFlyTutorialMessage();
        }
        public static void DisplayGlidePunishment()
        {
            if (_glidePunishmentTutorialCount++ != 0) return;
            Pause();
            UI.UI_GameMessage.DisplayGlidePunishmentTutorialMessage();
        }
        public static void DisplayPuzzleHint()
        {
            if (_puzzleHintTutorialCount++ != 0) return;
            Pause();
            UI.UI_GameMessage.DisplayPuzzleHintTutorialMessage();
        }
        public static void DisplayWaystoneTut()
        {
            if (_waystoneTutorialCount++ != 0) return;
            Pause();
            UI.UI_GameMessage.DisplayWaystoneTutorialMessage();
        }
    }
}

