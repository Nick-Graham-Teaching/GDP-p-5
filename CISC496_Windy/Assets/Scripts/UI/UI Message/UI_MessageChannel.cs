using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_MessageChannel<T, T1> : StaticSingleton<T1> where T : UI_Message where T1 : UI_MessageChannel<T, T1>
    {
        protected T MessageInstance;

        [SerializeField] protected float MessageFadeOutRate;
        [SerializeField] protected float MessageStayTime;
        Coroutine MessageFadeOutCoroutine;

        IEnumerator WindowFadeOut()
        {
            yield return new WaitForSeconds(MessageStayTime);
            yield return new WaitUntil(() => Util.ImageFadeOut(MessageInstance.Window, MessageFadeOutRate));
            MessageInstance.Window.enabled = false;
            Util.ResetImageAlpha(MessageInstance.Window, 1.0f);
            ResetMessageInstance();
        }

        protected void ApplyForShowup(T window)
        {
            if (MessageInstance.Available)
            {
                MessageInstance = window;
                MessageInstance.Window.enabled = true;
                MessageFadeOutCoroutine = StartCoroutine(WindowFadeOut());
            }
            else
            {
                StopCoroutine(MessageFadeOutCoroutine);
                Util.ResetImageAlpha(MessageInstance.Window, 1.0f);
                MessageInstance.Window.enabled = false;

                MessageInstance = window;
                MessageInstance.Window.enabled = true;
                MessageFadeOutCoroutine = StartCoroutine(WindowFadeOut());
            }
        }

        protected virtual void ResetMessageInstance() { }
        public static void TurnOffAllMessages()
        {
            if (!Instance.MessageInstance.Available)
            {
                Instance.StopCoroutine(Instance.MessageFadeOutCoroutine);
                Util.ResetImageAlpha(Instance.MessageInstance.Window, 1.0f);
                Instance.MessageInstance.Window.enabled = false;

                Instance.ResetMessageInstance();
            }
        }
    }
}

