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

        public static void ResetTutorialCount()
        {
            _flyTutorialCount = 0;
            _glidePunishmentTutorialCount = 0;
            _puzzleHintTutorialCount = 0;
            _mobileControlTutorialCount = 0;
        }

        IEnumerator ContinueKeySupervisor()
        {
            yield return new WaitUntil(() => Input.GetKeyDown(Keys.ContinueCode) || Controller.Controller.ControlDevice.GetKeyDown(Keys.ContinueCode, out float _));
            TurnOffTut();
        }

        static void Pause()
        {
            GameProgressManager.Instance.GameState = new Pause();
            UI.UIEventsHandler.Instance.PausePage.SetActive(false);
            Instance.PointerEventImage.enabled = true;
            Instance.StartCoroutine(Instance.ContinueKeySupervisor());
        }

        public static void DisplayMobilePhoneControlTutorial()
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
        public static void DisplayGlidePunishmentTutorial()
        {
            if (_glidePunishmentTutorialCount++ != 0) return;
            Pause();
            UI.UI_GameMessage.DisplayGlidePunishmentTutorialMessage();
        }
        public static void DisplayPuzzleHintTutorial()
        {
            if (_puzzleHintTutorialCount++ != 0) return;
            Pause();
            UI.UI_GameMessage.DisplayPuzzleHintTutorialMessage();
        }
        public static void TurnOffTut()
        {
            Instance.StopAllCoroutines();
            GameProgressManager.Instance.GameState = new Continue();
            Instance.PointerEventImage.enabled = false;
            UI.UI_GameMessage.TurnOffAllMessages();
        }


    }
}

