using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    // KeyboardInputHandler
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

            foreach (KeyCode keyCode in Keys.KeyboardKeys)
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
