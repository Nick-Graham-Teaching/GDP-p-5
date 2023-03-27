using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class GameMessage : UI_Message
    {
        public GameMessage() { }
        public GameMessage(UnityEngine.UI.Image image) : base(image) { }
    }

    public class VoidMessage : GameMessage
    {
        public override bool Available => true;
    }

    public class FlyTutorialMessage : GameMessage
    {
        public FlyTutorialMessage(UnityEngine.UI.Image image) : base(image) { }
    }

    public class PuzzleSolvedMessage : GameMessage
    {
        public PuzzleSolvedMessage(UnityEngine.UI.Image image) : base(image) { }
    }
}
