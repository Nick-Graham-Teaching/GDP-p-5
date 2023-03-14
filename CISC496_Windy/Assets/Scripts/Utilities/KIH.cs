using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public enum KEYSTAT
    {
        PRESS, TAP, IDLE, UP
    }

    public class Key
    {
        public KEYSTAT Value { get; set; }
        public Key()
        {
            Value = KEYSTAT.IDLE;
        }
    }

    public static class Keys
    {
        public static readonly KeyCode UpCode = KeyCode.W;   // Going forward
        public static readonly KeyCode LeftCode = KeyCode.A;   // Going left
        public static readonly KeyCode DownCode = KeyCode.S;   // Going backword
        public static readonly KeyCode RightCode = KeyCode.D;   // Going right
        public static readonly KeyCode JumpCode = KeyCode.Space;   // Jump
        public static readonly KeyCode ModeSwitchCode = KeyCode.LeftShift;   // ModeSwithc
        public static readonly KeyCode PauseCode = KeyCode.P;


        public static readonly KeyCode[] keys = { UpCode, LeftCode, DownCode, RightCode, JumpCode, ModeSwitchCode, PauseCode };
    }

    // Stand for KeyInputHandler
    public class KIH : MonoBehaviour
    {

        // After receving getkeydown event, wait for keyBufferCD seconds to see if getkey events get recveived.
        [SerializeField]
        private float DirectionKeyColdDown;
        [SerializeField]
        private float JumpKeyColdDown;

        private static Dictionary<KeyCode, Key> keyDic;
        private static Dictionary<KeyCode, float> pressingKeys;

        private void Start()
        {
            keyDic = new Dictionary<KeyCode, Key>();
            pressingKeys = new Dictionary<KeyCode, float>();

            foreach (KeyCode keyCode in Keys.keys)
            {
                keyDic.Add(keyCode, new Key());
                StartCoroutine(KeyboardControl(keyCode));
            }
        }

        private IEnumerator KeyboardControl(KeyCode key)
        {
            while (true)
            {
                if (Input.GetKeyDown(key))
                {
                    pressingKeys.Add(key, 0.0f);
                    StartCoroutine(KeyColdDownTimer(key));
                }
                yield return null;
            }
        }
        private IEnumerator KeyColdDownTimer(KeyCode key)
        {
            yield return new WaitUntil(() => {

                pressingKeys[key] += Time.deltaTime;

                return Input.GetKeyUp(key) || pressingKeys[key] >= (key==Keys.JumpCode ? JumpKeyColdDown : DirectionKeyColdDown);
            });

            keyDic[key].Value = Input.GetKey(key) ? KEYSTAT.PRESS : KEYSTAT.TAP;
            pressingKeys.Remove(key);

            if (keyDic[key].Value == KEYSTAT.PRESS)
            {
                StartCoroutine(WaitForKeyUp(key));
            }
            else
            {
                yield return null;
                keyDic[key].Value = KEYSTAT.UP;
                yield return null;
                keyDic[key].Value = KEYSTAT.IDLE;
            }
        }
        private IEnumerator WaitForKeyUp(KeyCode key)
        {
            yield return new WaitUntil(() => Input.GetKeyUp(key));
            keyDic[key].Value = KEYSTAT.UP;
            yield return null;
            keyDic[key].Value = KEYSTAT.IDLE;
        }


        public static bool GetKeyUp(KeyCode kc)
        {
            return keyDic[kc].Value == KEYSTAT.UP;
        }
        public static bool GetKeyTap(KeyCode kc)
        {
            return keyDic[kc].Value == KEYSTAT.TAP;
        }
        public static bool GetKeyPress(KeyCode kc)
        {
            return keyDic[kc].Value == KEYSTAT.PRESS;
        }
    }
}
