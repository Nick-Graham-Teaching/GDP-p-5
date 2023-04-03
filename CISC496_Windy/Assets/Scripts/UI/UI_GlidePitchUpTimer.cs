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

        Image[] AllImages;

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
                UseTimer.SetActive(false);
                CDTimer.SetActive(true);
                CDTimer_DisusableImage.fillAmount -= (1.0f / GlidePitchUpTimer.PunishmentCD) * Time.deltaTime;
                CDTimer_UsableImage.fillAmount = 1.0f - CDTimer_DisusableImage.fillAmount;
            }
            else
            {
                UseTimer.SetActive(true);
                CDTimer.SetActive(false);
                UseTimer_FillImage.fillAmount = Mathf.Lerp(UseTimer_FillImage.fillAmount, GlidePitchUpTimer.UseTimeRatio, ChangeRate * Time.deltaTime);
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

        public static void ResetImageFillAmount() 
        {
            Instance.CDTimer_DisusableImage.fillAmount = 1.0f;
            Instance.CDTimer_UsableImage.fillAmount = 0.0f;
        }
    }
}


