using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_GameMessage : UI_MessageChannel<GameMessage, UI_GameMessage>
    {

        VoidMessage VoidMessage;
        FlyTutorialMessage FlyTutorialMessage;
        PuzzleSolvedMessage PuzzleSolvedMessage;

        [SerializeField] UnityEngine.UI.Image FlyTutorialImage;
        [SerializeField] UnityEngine.UI.Image PuzzleSolvedImage;

        private void Start()
        {
            VoidMessage = new VoidMessage();
            FlyTutorialMessage = new FlyTutorialMessage(FlyTutorialImage);
            PuzzleSolvedMessage = new PuzzleSolvedMessage(PuzzleSolvedImage);

            ResetMessageInstance();
        }

        protected override void ResetMessageInstance()
        {
            MessageInstance = VoidMessage;
        }

        public static void DisplayFlyTutorialMessage()
        {
            Instance.ApplyForShowup(Instance.FlyTutorialMessage);
        }
        public static void DisplayPuzzleSolvedMessage()
        {
            Instance.ApplyForShowup(Instance.PuzzleSolvedMessage);
        }
    }
}

