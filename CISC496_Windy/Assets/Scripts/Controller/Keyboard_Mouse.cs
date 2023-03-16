using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Controller
{
    public class Keyboard_Mouse : IController
    {

        protected internal float DirectionKeyColdDown;
        protected internal float JumpKeyColdDown;

        private Dictionary<KeyCode, Key> keyDic;
        private Dictionary<KeyCode, float> pressingKeys;

        public Keyboard_Mouse()
        {
            keyDic = new Dictionary<KeyCode, Key>();
            pressingKeys = new Dictionary<KeyCode, float>();

            foreach (KeyCode keyCode in Keys.keys)
            {
                keyDic.Add(keyCode, new Key());
            }
        }

        public void Update()
        {
            foreach (KeyCode key in Keys.keys)
            {
                if (Input.GetKeyDown(key))
                {
                    pressingKeys.Add(key, 0.0f);
                    Controller.Instance.StartCoroutine(KeyColdDownTimer(key));
                }
            }
        }
        IEnumerator KeyColdDownTimer(KeyCode key)
        {
            yield return new WaitUntil(() => {

                pressingKeys[key] += Time.deltaTime;

                return Input.GetKeyUp(key) || pressingKeys[key] >= (key == Keys.JumpCode ? JumpKeyColdDown : DirectionKeyColdDown);
            });

            keyDic[key].Value = Input.GetKey(key) ? KEYSTAT.PRESS : KEYSTAT.TAP;
            pressingKeys.Remove(key);

            if (keyDic[key].Value == KEYSTAT.PRESS)
            {
                Controller.Instance.StartCoroutine(WaitForKeyUp(key));
            }
            else
            {
                yield return null;
                keyDic[key].Value = KEYSTAT.UP;
                yield return null;
                keyDic[key].Value = KEYSTAT.IDLE;
            }
        }
        IEnumerator WaitForKeyUp(KeyCode key)
        {
            yield return new WaitUntil(() => Input.GetKeyUp(key));
            keyDic[key].Value = KEYSTAT.UP;
            yield return null;
            keyDic[key].Value = KEYSTAT.IDLE;
        }

        public float GetCameraMoveAxisX()
        {
            return Input.GetAxis("Mouse X");
        }

        public float GetCameraMoveAxisY()
        {
            return Input.GetAxis("Mouse Y");
        }

        public bool GetKeyPress(KeyCode key, out float degree)
        {
            if (!keyDic.ContainsKey(key)) throw new UnknownKeyException("The key - " + key + " - is not available!");
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.PRESS;
        }

        public bool GetKeyTap(KeyCode key, out float degree)
        {
            if (!keyDic.ContainsKey(key)) throw new UnknownKeyException("The key - " + key + " - is not available!");
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.TAP;
        }

        public void Start() { }

        public void Quit() 
        {
            foreach (KeyCode key in keyDic.Keys)
            {
                keyDic[key].Value = KEYSTAT.IDLE;
            }
        }
    }
}

