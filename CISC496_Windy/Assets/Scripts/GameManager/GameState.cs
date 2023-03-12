using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Game
{
    public abstract class GameState
    {
        public GameState() { OnStateChange(); }
        protected abstract void OnStateChange();
        public abstract bool IsInGame();
    }


    public sealed class InGame : GameState
    {
        protected override void OnStateChange() { }
        public override bool IsInGame() => true;
    }


    public sealed class Continue : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnContinue?.Invoke();
            GameProgressManager.Instance.GameState = new InGame();
        }
        public override bool IsInGame() => true;
    }


    public sealed class Restart : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnRestart?.Invoke();
            GameProgressManager.Instance.GameState = new InGame();
        }
        public override bool IsInGame() => true;
    }


    public sealed class Pause : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnPause?.Invoke();
        }
        public override bool IsInGame() => false;
    }


    public sealed class Ready : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnToStartPage?.Invoke();
        }
        public override bool IsInGame() => false;
    }


    public sealed class GameOver : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnGameOver?.Invoke();
        }
        public override bool IsInGame() => false;
    }

}
