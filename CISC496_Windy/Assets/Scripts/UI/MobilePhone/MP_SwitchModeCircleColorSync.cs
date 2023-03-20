using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class MP_SwitchModeCircleColorSync : MonoBehaviour
    {

        public GameObject SwitchModeIcon;

        private UnityEngine.UI.Image _image;
        private UnityEngine.UI.Image CircleImage
        {
            get
            {
                if (_image is null)
                {
                    _image = GetComponent<UnityEngine.UI.Image>();
                }
                return _image;
            }
        }

        private Button _switchModeButton;
        private Button SwitchModeButton
        {
            get
            {
                if(_switchModeButton is null)
                {
                    _switchModeButton = SwitchModeIcon.GetComponent<Button>();
                }
                return _switchModeButton;
            }
        }

        public void StartSync()
        {
            StopAllCoroutines();
            StartCoroutine(OnSwitchModeButtonPointerDown());
        }
        public void FinishSync()
        {
            StopAllCoroutines();
            StartCoroutine(OnSwitchModeButtonPointerExit());
        }
        
        IEnumerator OnSwitchModeButtonPointerDown()
        {
            yield return new WaitUntil(() => Util.ImageColorLerp(CircleImage, 25f, SwitchModeButton.colors.pressedColor));
        }

        IEnumerator OnSwitchModeButtonPointerExit()
        {
            yield return new WaitUntil(() => Util.ImageColorLerp(CircleImage, 25f, SwitchModeButton.colors.normalColor));
        }
    }
}
