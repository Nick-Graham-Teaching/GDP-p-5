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
            else
            {
                UseTimer_FillImage.fillAmount = Mathf.Lerp(UseTimer_FillImage.fillAmount, GlidePitchUpTimer.UseTimeRatio, ChangeRate * Time.deltaTime);
            }
        }

        private void Start()
        {
            CDTimer.SetActive(false);
            UseTimer.SetActive(false);
        }

        public static void TurnOff()
        {
            if (Instance is null) return;
            Instance._isEnabled = false;
            Instance.CDTimer.SetActive(false);
            Instance.UseTimer.SetActive(false);
        }
        public static void TurnOn()
        {
            Instance._isEnabled = true;
            Instance.CDTimer.SetActive(false);
            Instance.UseTimer.SetActive(true);
        }

        public static void TurnOnCDTimer() 
        {
            Instance.CDTimer_DisusableImage.fillAmount = 1.0f;
            Instance.CDTimer_UsableImage.fillAmount = 0.0f;
            Instance.UseTimer.SetActive(false);
            Instance.CDTimer.SetActive(true);
        }

        public static void TurnOffCDTimer()
        {
            Instance.UseTimer.SetActive(true);
            Instance.CDTimer.SetActive(false);
        }
    }
}


