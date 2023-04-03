using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_MessageChannel<T> : StaticSingleton<T> where T : UI_MessageChannel<T>
    {
        protected UI_Message MessageInstance;

        protected VoidMessage _voidMessage;

        [SerializeField] protected float MessageFadeOutRate;
        Coroutine MessageFadeOutCoroutine;

        protected void Start()
        {
            _voidMessage = new VoidMessage();
            MessageInstance = _voidMessage;
        }

        IEnumerator WindowFadeOut()
        {
            yield return new WaitForSeconds(MessageInstance.DisplayTime);
            yield return new WaitUntil(() => Util.ImageFadeOut(MessageInstance.Window, MessageFadeOutRate));
            MessageInstance.Window.enabled = false;
            Util.ResetImageAlpha(MessageInstance.Window, 1.0f);
            ResetMessageInstance();
        }

        protected void ApplyForShowup(UI_Message window, bool touchable = false)
        {
            if (MessageInstance.Available)
            {
                MessageInstance = window;
                MessageInstance.Window.enabled = true;
                if (!touchable) MessageFadeOutCoroutine = StartCoroutine(WindowFadeOut());
            }
            else
            {
                if (Instance.MessageFadeOutCoroutine is not null) StopCoroutine(MessageFadeOutCoroutine);
                Util.ResetImageAlpha(MessageInstance.Window, 1.0f);
                MessageInstance.Window.enabled = false;

                MessageInstance = window;
                MessageInstance.Window.enabled = true;
                if (!touchable) MessageFadeOutCoroutine = StartCoroutine(WindowFadeOut());
            }
        }

        protected void ResetMessageInstance() => MessageInstance = _voidMessage;

        public static void TurnOffAllMessages()
        {
            if (!Instance.MessageInstance.Available)
            {
                if (Instance.MessageFadeOutCoroutine is not null) Instance.StopCoroutine(Instance.MessageFadeOutCoroutine);
                Util.ResetImageAlpha(Instance.MessageInstance.Window, 1.0f);
                Instance.MessageInstance.Window.enabled = false;

                Instance.ResetMessageInstance();
            }
        }
    }
}

