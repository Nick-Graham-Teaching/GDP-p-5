using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windy.UI
{
    using Buoyancy;

    public class UI_GlidePitchUpTimer : StaticSingleton<UI_GlidePitchUpTimer>
    {
        [SerializeField] GameObject CDTimer;
        [SerializeField] GameObject UseTimer;
        [SerializeField] GameObject DisabledTimerImage;

        [SerializeField] Image UseTimer_FillImage;
        [SerializeField] Image CDTimer_UsableImage;
        [SerializeField] Image CDTimer_DisusableImage;
        [SerializeField] float ChangeRate;
        [SerializeField] float FadeInOutRate;

        bool _isEnabled;

        private void LateUpdate()
        {
            if (!_isEnabled)
            {
                return;
            }
            if (GlidePitchUpTimer.IsExceedingTimeLimit)
            {
                CDTimer_DisusableImage.fillAmount -= (1.0f / GlidePitchUpTimer.PunishmentCD) * Time.deltaTime;
                CDTimer_UsableImage.fillAmount = 1.0f - CDTimer_DisusableImage.fillAmount;
            }
            else if (!MM_Executor.Instance.MotionMode.IsGlide())
            {
                if (DisabledTimerImage.activeSelf) return;
                CDTimer.SetActive(false);
                UseTimer.SetActive(false);
                DisabledTimerImage.SetActive(true);
            }
            else
            {
                UseTimer_FillImage.fillAmount = Mathf.Lerp(UseTimer_FillImage.fillAmount, GlidePitchUpTimer.UseTimeRatio, ChangeRate * Time.deltaTime);
            }
        }

        private void Start()
        {
            CDTimer.SetActive(false);
            UseTimer.SetActive(false);
            DisabledTimerImage.SetActive(false);
        }

        public static void TurnOff()
        {
            if (Instance is null) return;
            Instance._isEnabled = false;
            Instance.CDTimer.SetActive(false);
            Instance.UseTimer.SetActive(false);
            Instance.DisabledTimerImage.SetActive(false);
        }
        public static void TurnOn(bool isDive)
        {
            Instance._isEnabled = true;

            if (GlidePitchUpTimer.IsExceedingTimeLimit)
            {
                Instance.CDTimer.SetActive(true);
                Instance.UseTimer.SetActive(false);
                Instance.DisabledTimerImage.SetActive(false);
            }
            else
            {
                Instance.CDTimer.SetActive(false);
                Instance.UseTimer.SetActive(!isDive);
                Instance.DisabledTimerImage.SetActive(isDive);
            }
        }

        public static void TurnOnCDTimer() 
        {
            if (Instance._isEnabled)
            {
                Instance.CDTimer_DisusableImage.fillAmount = 1.0f;
                Instance.CDTimer_UsableImage.fillAmount = 0.0f;
                Instance.UseTimer.SetActive(false);
                Instance.CDTimer.SetActive(true);
            }
        }

        public static void TurnOffCDTimer()
        {
            if (Instance._isEnabled)
            {
                Instance.UseTimer.SetActive(true);
                Instance.CDTimer.SetActive(false);
            }
        }
    }
}


