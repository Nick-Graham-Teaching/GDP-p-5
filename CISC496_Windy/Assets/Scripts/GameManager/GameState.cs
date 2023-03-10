using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.GameState
{
    public abstract class GameState
    {
        public GameState() { OnStateChange(); }
        protected virtual void OnStateChange() { }

        public abstract bool IsInGame();
    }
    public sealed class InGame : GameState
    {
        public override bool IsInGame() { return true; }
    }
    public sealed class Continue : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnContinue?.Invoke();
            GameProgressManager.Instance.GameState = new InGame();
        }
        public override bool IsInGame() { return true; }
    }
    public sealed class Restart : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnRestart?.Invoke();
            GameProgressManager.Instance.GameState = new InGame();
        }
        public override bool IsInGame() { return true; }
    }
    public sealed class Pause : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnPause?.Invoke();
        }
        public override bool IsInGame() { return false; }
    }
    public sealed class Ready : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnToStartPage?.Invoke();
        }
        public override bool IsInGame() { return false; }
    }
    public sealed class GameOver : GameState
    {
        protected override void OnStateChange()
        {
            GameEvents.OnGameOver?.Invoke();
        }
        public override bool IsInGame() { return false; }
    }

}
