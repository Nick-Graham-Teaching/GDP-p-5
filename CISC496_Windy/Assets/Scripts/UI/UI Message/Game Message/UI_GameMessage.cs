using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_GameMessage : UI_MessageChannel<GameMessage, UI_GameMessage>
    {

        VoidMessage VoidMessage;
        FlyTutorialMessage _flyTutorialMessage;
        GlidePunishmentTutorialMessage _glidePunishmentTutorialMessage;
        PuzzleHintTutorialMessage _puzzleHintTutorialMessage;
        SwitchModeKeyTutorialMessage _switchModeKeyTutorialMessage;
        PuzzleSolvedMessage _puzzleSolvedMessage;

        [SerializeField] UnityEngine.UI.Image FlyTutorialImage;
        [SerializeField] float FlyTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image GlidePunishmentTutorialImage;
        [SerializeField] float GlidePunishmentTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image PuzzleHintTutorialImage;
        [SerializeField] float PuzzleHintTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image SwitchModeKeyTutorialImage;
        [SerializeField] float SwitchModeKeyTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image PuzzleSolvedImage;
        [SerializeField] float PuzzleSolvedStayTime;

        static int _glidePunishmentTutorialCount = 0;
        static int _puzzleHintTutorialCount = 0;
        static int _switchModeKeyTutorialCount = 0;

        private void Start()
        {
            VoidMessage = new VoidMessage();
            _flyTutorialMessage = new FlyTutorialMessage(FlyTutorialImage, FlyTutorialStayTime);
            _glidePunishmentTutorialMessage = new GlidePunishmentTutorialMessage(GlidePunishmentTutorialImage, GlidePunishmentTutorialStayTime);
            _puzzleHintTutorialMessage = new PuzzleHintTutorialMessage(PuzzleHintTutorialImage, PuzzleHintTutorialStayTime);
            _switchModeKeyTutorialMessage = new SwitchModeKeyTutorialMessage(SwitchModeKeyTutorialImage, SwitchModeKeyTutorialStayTime);
            _puzzleSolvedMessage = new PuzzleSolvedMessage(PuzzleSolvedImage, PuzzleSolvedStayTime);

            ResetMessageInstance();
        }

        protected override void ResetMessageInstance()
        {
            MessageInstance = VoidMessage;
        }

        public static void ResetTutorialCound()
        {
            _glidePunishmentTutorialCount = 0;
            _puzzleHintTutorialCount = 0;
            _switchModeKeyTutorialCount = 0;
        }

        public static void DisplayFlyTutorialMessage()
        {
            Instance.ApplyForShowup(Instance._flyTutorialMessage);
        }
        public static void DisplayGlidePunishmentTutorialMessage()
        {
            if (_glidePunishmentTutorialCount++ == 0)
                Instance.ApplyForShowup(Instance._glidePunishmentTutorialMessage);
        }
        public static void DisplayPuzzleHintTutorialMessage()
        {
            if (_puzzleHintTutorialCount++ == 0)
                Instance.ApplyForShowup(Instance._puzzleHintTutorialMessage);
        }
        public static void DisplaySwitchModeKeyTutorialMessage()
        {
            if (_switchModeKeyTutorialCount++ == 0)
                Instance.ApplyForShowup(Instance._switchModeKeyTutorialMessage);
        }
        
        public static void DisplayPuzzleSolvedMessage()
        {
            Instance.ApplyForShowup(Instance._puzzleSolvedMessage);
        }
    }
}

