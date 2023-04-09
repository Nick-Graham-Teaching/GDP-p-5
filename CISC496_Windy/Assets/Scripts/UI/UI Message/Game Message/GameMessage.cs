using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class GameMessage : UI_Message
    {
        public GameMessage() { }
        public GameMessage(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }

    public class FlyTutorialMessage : GameMessage
    {
        public FlyTutorialMessage(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }

    public class GlidePunishmentTutorialMessage : GameMessage
    {
        public GlidePunishmentTutorialMessage(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }

    public class PuzzleHintTutorialMessage : GameMessage
    {
        public PuzzleHintTutorialMessage(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }

    [System.Obsolete]
    public class SwitchModeKeyTutorialMessage: GameMessage
    {
        public SwitchModeKeyTutorialMessage(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }

    public class MobilePhoneControlTutorialMessage : GameMessage
    {
        public MobilePhoneControlTutorialMessage(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }
    public class WaystoneTutorialMessage : GameMessage
    {
        public WaystoneTutorialMessage(UnityEngine.UI.Image image, float stayTime):base(image, stayTime) { }
    }

    public class PuzzleSolvedMessage : GameMessage
    {
        public PuzzleSolvedMessage(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }
}
