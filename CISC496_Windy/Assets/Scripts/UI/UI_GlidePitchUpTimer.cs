using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windy.UI
{
    using Buoyancy;

    public class UI_GlidePitchUpTimer : StaticSingleton<UI_GlidePitchUpTimer>
    {
        Image[] AllImages;

        [SerializeField] Image FillImage;
        [SerializeField] Color NormalImageColor;
        [SerializeField] Color PunishmentImageColor;
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
                FillImage.color = PunishmentImageColor;
                FillImage.fillAmount -= (1.0f / GlidePitchUpTimer.PunishmentCD) * Time.deltaTime;
            }
            else
            {
                FillImage.color = NormalImageColor;
                FillImage.fillAmount = Mathf.Lerp(FillImage.fillAmount, GlidePitchUpTimer.UseTimeRatio, ChangeRate * Time.deltaTime);
            }
        }

        private void Start()
        {
            AllImages = GetComponentsInChildren<Image>();
        }

        public static void TurnOff() 
        {
            if (Instance is null) return;
            Instance._isEnabled = false;
            foreach (Image image in Instance.AllImages)
            {
                image.enabled = false;
            }
        }
        public static void TurnOn()
        {
            Instance._isEnabled = true;
            foreach (Image image in Instance.AllImages)
            {
                image.enabled = true;
            }
        }

        public static void ResetImageFillAmount() => Instance.FillImage.fillAmount = 1.0f;
    }
}


