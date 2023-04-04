using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Windy.UI
{
    public class UI_GameMessage : UI_MessageChannel<UI_GameMessage>
    {
        FlyTutorialMessage _flyTutorialMessage;
        GlidePunishmentTutorialMessage _glidePunishmentTutorialMessage;
        PuzzleHintTutorialMessage _puzzleHintTutorialMessage;
        //SwitchModeKeyTutorialMessage _switchModeKeyTutorialMessage;
        MobilePhoneControlTutorialMessage _mobilePhoneControlTutorialMessage;

        PuzzleSolvedMessage _puzzleSolvedMessage;

        [SerializeField] UnityEngine.UI.Image FlyTutorialImage;
        [SerializeField] float FlyTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image GlidePunishmentTutorialImage;
        [SerializeField] float GlidePunishmentTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image PuzzleHintTutorialImage;
        [SerializeField] float PuzzleHintTutorialStayTime;
        //[SerializeField] UnityEngine.UI.Image SwitchModeKeyTutorialImage;
        [SerializeField] UnityEngine.UI.Image MobilePhoneControlTutorialMessage;
        //[SerializeField] float SwitchModeKeyTutorialStayTime;
        [SerializeField] float MobilePhoneControlTutorialStayTime;

        [SerializeField] UnityEngine.UI.Image PuzzleSolvedImage;
        [SerializeField] float PuzzleSolvedStayTime;

        private new void Start()
        {
            base.Start();
            _flyTutorialMessage = new FlyTutorialMessage(FlyTutorialImage, FlyTutorialStayTime);
            _glidePunishmentTutorialMessage = new GlidePunishmentTutorialMessage(GlidePunishmentTutorialImage, GlidePunishmentTutorialStayTime);
            _puzzleHintTutorialMessage = new PuzzleHintTutorialMessage(PuzzleHintTutorialImage, PuzzleHintTutorialStayTime);
            _mobilePhoneControlTutorialMessage = new MobilePhoneControlTutorialMessage(MobilePhoneControlTutorialMessage, MobilePhoneControlTutorialStayTime);
            _puzzleSolvedMessage = new PuzzleSolvedMessage(PuzzleSolvedImage, PuzzleSolvedStayTime);
        }
        

        public static void DisplayFlyTutorialMessage()
        {
            Instance.ApplyForShowup(Instance._flyTutorialMessage, true);
        }
        public static void DisplayGlidePunishmentTutorialMessage()
        {
            Instance.ApplyForShowup(Instance._glidePunishmentTutorialMessage, true);
        }
        public static void DisplayPuzzleHintTutorialMessage()
        {
            Instance.ApplyForShowup(Instance._puzzleHintTutorialMessage, true);
        }
        public static void DisplayMobileControllerTutorialMessage()
        {
            Instance.ApplyForShowup(Instance._mobilePhoneControlTutorialMessage, true);
        }

        public static void DisplayPuzzleSolvedMessage()
        {
            Instance.ApplyForShowup(Instance._puzzleSolvedMessage);
        }
    }
}

