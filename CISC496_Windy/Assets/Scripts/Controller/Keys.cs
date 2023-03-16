using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public enum KEYSTAT
    {
        PRESS, TAP, IDLE, UP, DOWN, WAIT
    }

    public class Key
    {
        public KEYSTAT Value { get; set; }

        public float degree { get; set; }

        public Key()
        {
            Value = KEYSTAT.IDLE;
            degree = 1.0f;
        }
    }

    public static class Keys
    {
        public static readonly KeyCode UpCode = KeyCode.W;                   // Going forward
        public static readonly KeyCode LeftCode = KeyCode.A;                 // Going left
        public static readonly KeyCode DownCode = KeyCode.S;                 // Going backword
        public static readonly KeyCode RightCode = KeyCode.D;                // Going right
        public static readonly KeyCode JumpCode = KeyCode.Space;             // Jump
        public static readonly KeyCode ModeSwitchCode = KeyCode.LeftShift;   // ModeSwitch
        public static readonly KeyCode PauseCode = KeyCode.P;                // Pause
        public static readonly KeyCode ContinueCode = KeyCode.P;                // Continue

        public static readonly KeyCode[] keys = { UpCode, LeftCode, DownCode, RightCode, JumpCode, ModeSwitchCode, PauseCode };
    }
}

