using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Game
{
    public abstract class GameState : IBehaviour
    {
        public GameState() { OnStateChange(); }

        protected virtual void OnStateChange() { }
        public abstract bool IsInGame();

        public virtual void Start(){ }

        public virtual void Update() { }

        public virtual void Quit() {}
    }


    public class InGame : GameState
    {
        public override void Update()
        {
            //if (Input.GetKeyDown(Keys.PauseCode))
            if (Input.GetKeyDown(Keys.PauseCode) || Controller.Controller.ControlDevice.GetKeyDown(Keys.PauseCode, out float _))
            {
                GameProgressManager.Instance.GameState = new Pause();
            }
        }
        public override bool IsInGame() => true;

        public override string ToString() => "InGame";
    }


    public sealed class Continue : InGame
    {

        protected override void OnStateChange()
        {
            GameEvents.OnContinue?.Invoke();
        }
        public override string ToString() => "Continue";
    }


    public sealed class Restart : InGame
    {
        protected override void OnStateChange()
        {
            GameEvents.OnRestart?.Invoke();
        }
        public override string ToString() => "Restart";
    }

    public class OutOfGame : GameState
    {
        public override bool IsInGame() => false;
        public override string ToString() => "OutOfGame";
    }


    public sealed class Pause : OutOfGame
    {
        public override void Update()
        { 
            if (Input.GetKeyDown(Keys.ContinueCode) || Controller.Controller.ControlDevice.GetKeyDown(Keys.ContinueCode, out float _))
            {
                if (!GameTutorialManager.IsDisplayAll)
                    GameProgressManager.Instance.GameState = new Continue();
            }
        }
        protected override void OnStateChange()
        {
            GameEvents.OnPause?.Invoke();
        }
        public override string ToString() => "Pause";
    }


    public sealed class Ready : OutOfGame
    {
        protected override void OnStateChange()
        {
            GameEvents.OnToStartPage?.Invoke();
        }
        public override string ToString() => "Ready";
    }


    public sealed class GameOver : OutOfGame
    {
        protected override void OnStateChange()
        {
            GameEvents.OnGameOver?.Invoke();
        }
        public override string ToString() => "GameOver";
    }

}
