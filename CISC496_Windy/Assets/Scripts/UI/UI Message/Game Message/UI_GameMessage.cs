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
        MobilePhoneControlTutorialMessage _mobilePhoneControlTutorialMessage;
        //WaystoneTutorialMessage _waystoneTutorialMessage;

        static int _displayAllIndex = 0;
        static GameMessage[] _allTutMessages;

        PuzzleSolvedMessage _puzzleSolvedMessage;

        [SerializeField] UnityEngine.UI.Image FlyTutorialImage;
        [SerializeField] float FlyTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image GlidePunishmentTutorialImage;
        [SerializeField] float GlidePunishmentTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image PuzzleHintTutorialImage;
        [SerializeField] float PuzzleHintTutorialStayTime;
        [SerializeField] UnityEngine.UI.Image MobilePhoneControlTutorialImage;
        [SerializeField] float MobilePhoneControlTutorialStayTime;
        //[SerializeField] UnityEngine.UI.Image WaystoneTutorialImage;
        //[SerializeField] float WaystoneTutorialStayTime;

        [SerializeField] UnityEngine.UI.Image PuzzleSolvedImage;
        [SerializeField] float PuzzleSolvedStayTime;

        private new void Start()
        {
            base.Start();
            _flyTutorialMessage =                new FlyTutorialMessage                (FlyTutorialImage,                FlyTutorialStayTime);
            _glidePunishmentTutorialMessage =    new GlidePunishmentTutorialMessage    (GlidePunishmentTutorialImage,    GlidePunishmentTutorialStayTime);
            _puzzleHintTutorialMessage =         new PuzzleHintTutorialMessage         (PuzzleHintTutorialImage,         PuzzleHintTutorialStayTime);
            _mobilePhoneControlTutorialMessage = new MobilePhoneControlTutorialMessage (MobilePhoneControlTutorialImage, MobilePhoneControlTutorialStayTime);
            //_waystoneTutorialMessage =           new WaystoneTutorialMessage           (WaystoneTutorialImage,           WaystoneTutorialStayTime);

            _puzzleSolvedMessage = new PuzzleSolvedMessage(PuzzleSolvedImage, PuzzleSolvedStayTime);

            _allTutMessages = new GameMessage[] { 
                _mobilePhoneControlTutorialMessage,
                _flyTutorialMessage,
                _puzzleHintTutorialMessage,
                //_waystoneTutorialMessage,
                _glidePunishmentTutorialMessage
            };
        }

        public static bool DisplayAll()
        {
            if (_displayAllIndex == _allTutMessages.Length )
            {
                _displayAllIndex = 0;
                return false;
            }
            Instance.ApplyForShowup(_allTutMessages[_displayAllIndex++], true);
            return true;
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
        [System.Obsolete]
        public static void DisplayWaystoneTutorialMessage()
        {
            throw new System.NotImplementedException();
            //Instance.ApplyForShowup(Instance._waystoneTutorialMessage, true);
        }

        public static void DisplayPuzzleSolvedMessage()
        {
            Instance.ApplyForShowup(Instance._puzzleSolvedMessage);
        }
    }
}

