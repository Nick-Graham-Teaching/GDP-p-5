using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{

    public static class MP_UIEvents
    {
        public static Action OnContinueActive;
        public static Action OnPauseActive;

        public static Action OnToInAirMode;
        public static Action OnBackToGroundMode;
    }

    public class MP_UIEventsHandler : Singleton<MP_UIEventsHandler>
    {

        public GameObject Continue;
        public GameObject Pause;

        public UnityEngine.Animation Anim_Jump;
        public UnityEngine.Animation Anim_SwitchMode;

        public string Anim_Jump_ToFlying;
        public string Anim_Jump_ToGround;
        public string Anim_SwitchMode_ToFlying;
        public string Anim_SwitchMode_ToGround;

        private void Start()
        {
            MP_UIEvents.OnContinueActive += () => {
                Continue.SetActive(true);
                Pause.SetActive(false);
            };
            MP_UIEvents.OnPauseActive += () => {
                Continue.SetActive(false);
                Pause.SetActive(true);
            };
            
            MP_UIEvents.OnToInAirMode += () => {
                //StartCoroutine(WaitUntilAnimationFinish(Anim_Jump_ToFlying, Anim_SwitchMode_ToFlying));
                StartCoroutine(WaitUntilAnimationFinish(Anim_SwitchMode_ToFlying));
            };
            MP_UIEvents.OnBackToGroundMode += () => {
                //StartCoroutine(WaitUntilAnimationFinish(Anim_Jump_ToGround, Anim_SwitchMode_ToGround));
                StartCoroutine(WaitUntilAnimationFinish(Anim_SwitchMode_ToGround));
            };
        }

        //IEnumerator WaitUntilAnimationFinish(string animOne, string animTwo)
        //{
        //    yield return new WaitWhile(() => Anim_Jump.isPlaying || Anim_SwitchMode.isPlaying);
        //    Anim_Jump.Play(animOne);
        //    Anim_SwitchMode.Play(animTwo);
        //}

        IEnumerator WaitUntilAnimationFinish(string animOne)
        {
            yield return new WaitWhile(() => Anim_SwitchMode.isPlaying);
            Anim_SwitchMode.Play(animOne);
        }
        public void ToAir() => MP_UIEvents.OnToInAirMode();
        public void ToGround() => MP_UIEvents.OnBackToGroundMode();
    }
}

