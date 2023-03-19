using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windy.UI
{
    public class UI_EnergySys : MonoBehaviour
    {

        Image _imageToControl;
        float _fillAmount;
        public float decreaseRate;


        private void Update()
        {
            _imageToControl.fillAmount = Mathf.Lerp(_imageToControl.fillAmount, _fillAmount, decreaseRate * Time.deltaTime);
        }

        private void Start()
        {
            _imageToControl = GetComponent<Image>();
            _fillAmount = _imageToControl.fillAmount;

            EnergySystem.EnergySys.Instance.EnergyChanged += (a) => _fillAmount = a;

            Game.GameEvents.OnToStartPage += OnResetStatus;
            Game.GameEvents.OnRestart     += OnResetStatus;
        }

        void OnResetStatus() => _imageToControl.fillAmount = 1.0f;
    }
}

